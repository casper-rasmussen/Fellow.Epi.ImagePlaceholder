using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Cms.Shell.UI.Rest.Projects;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Validation;
using Fellow.Epi.ImagePlaceholder.Manager.Placeholder;
using Fellow.Epi.ImagePlaceholder.Models.Media;

namespace Fellow.Epi.ImagePlaceholder.Infrastructure.Editor.Content
{
	class PlaceholderImageContentProvider : DefaultContentProvider
	{
		private readonly IBlobFactory _blobFactory;
		private readonly IValidationService _validationService;
		private readonly IProjectService _projectService;
		private readonly ISiteConfigurationRepository _siteConfigurationRepository;
		private readonly Settings _settings;
		private readonly IImageManager _placeholderManager;
		private readonly ContentMediaResolver _contentMediaResolver;
		private readonly IContentTypeRepository _contentTypeRepository;

		public PlaceholderImageContentProvider(ContentStore contentStore, ServiceAccessor<IPropertyDefinitionRepository> propertyDefinitionRepository, DefaultContentVersionRepository defaultContentVersionRepository, ServiceAccessor<IPageQuickSearch> pageQuickSearch, ServiceAccessor<ContentAclDB> contentAclDB, IPrincipalAccessor principalAccessor, IBlobFactory blobFactory, IValidationService validationService, IProjectService projectService, ISiteConfigurationRepository siteConfigurationRepository, Settings settings, IImageManager placeholderManager, ContentMediaResolver contentMediaResolver, IContentTypeRepository contentTypeRepository)
			: base(contentStore, defaultContentVersionRepository, propertyDefinitionRepository, pageQuickSearch, contentAclDB, principalAccessor)
		{
			this._blobFactory = blobFactory;
			this._validationService = validationService;
			this._projectService = projectService;
			this._siteConfigurationRepository = siteConfigurationRepository;
			this._settings = settings;
			this._placeholderManager = placeholderManager;
			this._contentMediaResolver = contentMediaResolver;
			this._contentTypeRepository = contentTypeRepository;

		}
		public override ContentReference Save(IContent content, SaveAction action)
		{
			//We know it's only possible to create placeholders as these are not persisted
			if (content is ImagePlaceholderMediaType)
			{
				ImagePlaceholderMediaType placeholderImage = content as ImagePlaceholderMediaType;

				//Get the type holding PNG files
				Type modelType = this._contentMediaResolver.GetFirstMatching(this._placeholderManager.FileExtension);

				ContentType contentType = this._contentTypeRepository.Load(modelType);

				//Get parent content
				IContent parentContent = this.LoadContent(content.ParentLink, LanguageSelector.MasterLanguage());

				//Create a new media item
				MediaData newMedia = this.GetDefaultContent(parentContent, contentType.ID, null) as MediaData;
				
				if (newMedia == null)
					return null;

				newMedia.Name = content.Name;
				newMedia.RouteSegment = placeholderImage.RouteSegment;

				Blob blob = newMedia.BinaryData;

				//Create a writeable blob if none
				if(blob == null)
					blob = this._blobFactory.CreateBlob(newMedia.BinaryDataContainer, this._placeholderManager.FileExtension);

				//Generate placeholder
				using(Stream placeholder = this._placeholderManager.GetStream(placeholderImage.Width, placeholderImage.Height)) 
				{
					//Assign placeholder stream to blob
					blob.Write(placeholder);
				}

				newMedia.BinaryData = blob;

				IEnumerable<ValidationError> validationFeedback = this._validationService.Validate(newMedia);

				//Check if there were any errors
				bool invalid = validationFeedback.Any(error => error.Severity == ValidationErrorSeverity.Error);

				SaveAction saveAction = SaveAction.None | (!invalid && !this._projectService.IsInProjectMode && this._siteConfigurationRepository.GetAutoPublishMediaOnUpload() ? SaveAction.Publish : SaveAction.Save);
				
				if (this._projectService.IsInProjectMode)
				{
					saveAction = saveAction | SaveAction.SkipSetCommonDraft;
				}

				newMedia.SetChangedOnPublish = this._settings.UIDefaultValueForSetChangedOnPublish;

				return base.Save(newMedia, saveAction);

			}

			return base.Save(content, action);
		}
	}
}
