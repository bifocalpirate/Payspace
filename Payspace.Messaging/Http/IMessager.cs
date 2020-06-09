using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Payspace.Messaging.Http
{
    public interface IMessager
    {
        Task<TResult> Post<TPayload, TResult>(string url, TPayload payload, string jwtToken = null);
        Task<TResult> Put<TPayload, TResult>(string url, TPayload payload, string jwtToken = null);
        Task<TResult> Get<TResult>(string url, string jwtToken = null);
        Task<TResult> Delete<TResult>(string url, string jwtToken=null);
    }
}
