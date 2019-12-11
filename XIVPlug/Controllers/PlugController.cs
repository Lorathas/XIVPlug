using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using XIVPlug.Models;
using Exception = System.Exception;

namespace XIVPlug.Controllers
{
    public class PlugController : ApiController
    {
        private readonly ToyConnection connection;

        public PlugController()
        {
            connection = ToyConnection.GetInstance();
        }

        [HttpPost]
        [Route("api/plugs/set")]
        public void Set([FromBody] object value)
        {
            Console.WriteLine(value);
        }

        [HttpGet]
        [Route("api/plugs/initialize")]
        public IHttpActionResult Get([FromUri] double value)
        {
            Console.WriteLine(value);

            return Ok(value);
        }

        [HttpPost]
        [Route("api/plugs/connect")]
        public async void Connect([FromBody] string name)
        {
            var client = connection.GetClientInstance();

            if (!client.Connected)
            {
                await client.ConnectAsync();
            }

            
        }

        [HttpPost]
        [Route("api/plugs/startscanning")]
        public async Task<IHttpActionResult> StartScanning()
        {
            var client = connection.GetClientInstance();

            if (!client.Connected)
            {
                await client.ConnectAsync();
            }

            try
            {
                await client.StartScanningAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/stopscanning")]
        public async Task<IHttpActionResult> StopScanning()
        {
            var client = connection.GetClientInstance();

            if (!client.Connected)
            {
                await client.ConnectAsync();
            }

            try
            {
                await client.StopScanningAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/plugs/list")]
        public async Task<IHttpActionResult> GetListOfDevices()
        {
            var client = connection.GetClientInstance();

            if (!client.Connected)
            {
                await client.ConnectAsync();
            }

            return Ok(client.Devices.Select(s => new Device(s)));
        }

        [HttpPost]
        [Route("api/plugs/select")]
        public async Task<IHttpActionResult> SelectDevice([FromBody] Device device)
        {
            connection.SelectedDeviceName = device.Name;

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/deselect")]
        public async Task<IHttpActionResult> DeselectDevice()
        {
            connection.SelectedDeviceName = null;

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/test")]
        public async Task<IHttpActionResult> TestDevice()
        {
            connection.SelectedDevice?.SendVibrateCmd(0.125);
            Thread.Sleep(500);
            connection.SelectedDevice?.SendVibrateCmd(0.0);

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/queueaction")]
        public IHttpActionResult QueueAction([FromBody] Command command)
        {
            connection.Enqueue(command);

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/queueactions")]
        public IHttpActionResult QueueActions([FromBody] List<Command> commands)
        {
            connection.Enqueue(commands);

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/clearqueue")]
        public IHttpActionResult ClearQueue()
        {
            connection.ClearQueue();

            return Ok();
        }
    }
}