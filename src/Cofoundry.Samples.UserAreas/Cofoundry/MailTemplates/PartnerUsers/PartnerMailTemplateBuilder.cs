﻿using Cofoundry.Core.Mail;
using Cofoundry.Domain.MailTemplates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Samples.UserAreas.PartnerMailTemplates
{
    /// <summary>
    /// An example of how to completely customize mail templates generated
    /// by Cofoundry for a user area. 
    /// 
    /// Each method represents a specific email sent out by the system and 
    /// should return an IMailTemplate instance which will be rendered by 
    /// Cofoundry. The methods are async so you have the freedom to fetch 
    /// and mix in any additional data you need.
    /// </summary>
    public class PartnerMailTemplateBuilder : IUserMailTemplateBuilder<PartnerUserAreaDefinition>
    { 
        public Task<IMailTemplate> BuildNewUserWithTemporaryPasswordTemplateAsync(NewUserWithTemporaryPasswordTemplateBuilderContext context)
        {
            var template = new NewUserWithTemporaryPasswordMailTemplate()
            {
                Username = context.User.Username,
                LoginPath = UrlLibrary.PartnerLogin(),
                TemporaryPassword = context.TemporaryPassword
            };

            return Task.FromResult<IMailTemplate>(template);
        }

        public Task<IMailTemplate> BuildPasswordResetByAdminTemplateAsync(PasswordResetByAdminTemplateBuilderContext context)
        {
            var template = new PasswordResetByAdminMailTemplate()
            {
                Username = context.User.Username,
                LoginPath = UrlLibrary.PartnerLogin(),
                TemporaryPassword = context.TemporaryPassword
            };

            return Task.FromResult<IMailTemplate>(template);
        }

        public Task<IMailTemplate> BuildPasswordResetRequestedByUserTemplateAsync(PasswordResetRequestedByUserTemplateBuilderContext context)
        {
            var resetUrl = "";// todo GetPasswordResetUrl(_userAreaDefinition, context);

            var template = new PasswordResetRequestedByUserMailTemplate()
            {
                Username = context.User.Username,
                ResetUrl = resetUrl
            };

            return Task.FromResult<IMailTemplate>(template);
        }

        public Task<IMailTemplate> BuildPasswordChangedTemplateAsync(PasswordChangedTemplateBuilderContext context)
        {
            var template = new PasswordChangedMailTemplate()
            {
                Username = context.User.Username,
                LoginPath = UrlLibrary.PartnerLogin()
            };

            return Task.FromResult<IMailTemplate>(template);
        }

        //protected string GetPasswordResetUrl(IUserAreaDefinition userAreaDefinition, PasswordResetRequestedByUserTemplateBuilderContext context)
        //{
        //    var baseUrl = RelativePathHelper.Combine(userAreaDefinition.LoginPath, "reset-password");

        //    return string.Format("{0}?i={1}&t={2}",
        //        baseUrl,
        //        context.UserPasswordResetRequestId.ToString("N"),
        //        Uri.EscapeDataString(context.Token));
        //}
    }
}