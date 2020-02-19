using System;
using System.Collections.Generic;
using Our.Umbraco.RedirectsViewer.Models.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Our.Umbraco.RedirectsViewer.Services
{
    public interface IOurRedirectsService
    {
        void Delete(Guid id,Guid contentId,string culture);
        
        IEnumerable<IRedirectUrl> GetContentRedirectUrls(Guid contentKey);
        
        IEnumerable<IRedirectUrl> GetAllRedirectUrls(long pageIndex, int pageSize, out long total);
        
        void Register(string url, Guid contentKey, string culture = null);
        
    }

    public class RedirectsService:IOurRedirectsService
    {
        private readonly IRedirectUrlService _redirectUrlService;
        public event EventHandler<RedirectAddedArgs> RedirectAdded;
        public event EventHandler<RedirectDeletedArgs> RedirectDeleted;
        public event EventHandler<RedirectDeletingArgs> RedirectDeleting;

        public RedirectsService(IRedirectUrlService redirectUrlService)
        {
            _redirectUrlService = redirectUrlService;
        }

        public void Delete(Guid id,Guid contentId,string culture)
        {
            RedirectDeleting?.Invoke(this,new RedirectDeletingArgs(contentId,culture));
            
            _redirectUrlService.Delete(id);
            
            RedirectDeleted?.Invoke(this,new RedirectDeletedArgs(contentId,culture));
        }

        public IEnumerable<IRedirectUrl> GetContentRedirectUrls(Guid contentKey)
        {
            return _redirectUrlService.GetContentRedirectUrls(contentKey);
        }

        public IEnumerable<IRedirectUrl> GetAllRedirectUrls(long pageIndex, int pageSize, out long total)
        {
            return _redirectUrlService.GetAllRedirectUrls(pageIndex, pageSize, out total);
        }

        public void Register(string url, Guid contentKey, string culture = null)
        {
            _redirectUrlService.Register(url,contentKey,culture);
            RedirectAdded?.Invoke(this,new RedirectAddedArgs(url,contentKey,culture));
        }
    }
    
}