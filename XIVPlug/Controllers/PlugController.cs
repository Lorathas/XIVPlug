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
            var client = ToyConnection.GetInstance();

            if (!client.Connected)
            {
                await client.ConnectAsync();
            }

            
        }

        [HttpPost]
        [Route("api/plugs/startscanning")]
        public async Task<IHttpActionResult> StartScanning()
        {
            var client = ToyConnection.GetInstance();

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
            var client = ToyConnection.GetInstance();

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
            var client = ToyConnection.GetInstance();

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
            ToyConnection.SelectedDeviceName = device.Name;

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/deselect")]
        public async Task<IHttpActionResult> DeselectDevice()
        {
            ToyConnection.SelectedDeviceName = null;

            return Ok();
        }

        [HttpPost]
        [Route("api/plugs/test")]
        public async Task<IHttpActionResult> TestDevice()
        {
            ToyConnection.SelectedDevice?.SendVibrateCmd(0.25);
            Thread.Sleep(500);
            ToyConnection.SelectedDevice?.SendVibrateCmd(0.0);

            return Ok();
        }
    }
}