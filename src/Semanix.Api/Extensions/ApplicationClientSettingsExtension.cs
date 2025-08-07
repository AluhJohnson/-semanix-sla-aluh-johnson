
namespace SlipFree.Api.Extensions
{
    public static class ApplicationClientSettingsExtension
    {
        public static IServiceCollection AddAppClientSettings(this IServiceCollection services, IConfiguration _config)
        {            
            
            //services
            //.AddOptions<InterBankTransaferClientSettings>()
            //.BindConfiguration("InterBankTransaferClientSettings")
            //.ValidateDataAnnotations(); 
            
            //services.Configure<AccountEnquiryClientSettings>(_config.GetSection("AccountEnquiryClientSettings"));
            return services;
        }
    }
}
