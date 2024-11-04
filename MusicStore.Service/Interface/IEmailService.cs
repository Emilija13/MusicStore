using MusicStore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage allMails);
    }
}
