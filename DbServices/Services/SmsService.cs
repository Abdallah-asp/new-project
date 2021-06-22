using DbServices.Base;
using Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace DbServices.Services
{
    public class SmsService : BaseService, ISms
    {
        public async Task<bool> SendMessage(string phone, string body)
        {
            var accountSid = "ACe1f9f58a067537c99df8d0e6361f4abc";
            var authToken = "7f35bbb6aaf7142c363ccf45e73db1cd";
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(phone);
            var from = new PhoneNumber("(404) 549-6280");

            var message = await MessageResource.CreateAsync(to: to, from: from, body: body);

            if (message.Sid == null)
            {
                return false;
            }

            return true;
        }
    }
}
