﻿using Cofoundry.Domain.Extendable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cofoundry.Domain
{
    public class AdvancedContentRepositoryCustomEntityRepository
            : IAdvancedContentRepositoryCustomEntityRepository
            , IExtendableContentRepositoryPart
    {
        private readonly ICustomEntityDefinitionRepository _customEntityDefinitionRepository;

        public AdvancedContentRepositoryCustomEntityRepository(
            IExtendableContentRepository contentRepository
            )
        {
            ExtendableContentRepository = contentRepository;
            _customEntityDefinitionRepository = contentRepository.ServiceProvider.GetRequiredService<ICustomEntityDefinitionRepository>();
        }

        public IExtendableContentRepository ExtendableContentRepository { get; }

        #region queries

        public IContentRepositoryCustomEntityGetAllQueryBuilder GetAll(string customEntityDefinitionCode)
        {
            return new ContentRepositoryCustomEntityGetAllQueryBuilder(ExtendableContentRepository, customEntityDefinitionCode);
        }

        public IContentRepositoryCustomEntityGetAllQueryBuilder GetAll<TDefinition>() where TDefinition : ICustomEntityDefinition
        {
            var customEntityDefinition = _customEntityDefinitionRepository.Get<TDefinition>();

            if (customEntityDefinition == null)
            {
                throw new Exception("Custom Entity Definition not returned from ICustomEntityDefinitionRepository: " + typeof(TDefinition).FullName);
            }

            return new ContentRepositoryCustomEntityGetAllQueryBuilder(ExtendableContentRepository, customEntityDefinition.CustomEntityDefinitionCode);
        }

        public IAdvancedContentRepositoryCustomEntityByIdQueryBuilder GetById(int customEntityId)
        {
            return new ContentRepositoryCustomEntityByIdQueryBuilder(ExtendableContentRepository, customEntityId);
        }

        public IAdvancedContentRepositoryCustomEntityByIdRangeQueryBuilder GetByIdRange(IEnumerable<int> pageIds)
        {
            return new ContentRepositoryCustomEntityByIdRangeQueryBuilder(ExtendableContentRepository, pageIds);
        }

        public IAdvancedContentRepositoryCustomEntitySearchQueryBuilder Search()
        {
            return new ContentRepositoryCustomEntitySearchQueryBuilder(ExtendableContentRepository);
        }

        public Task<bool> IsUrlSlugUniqueAsync(IsCustomEntityUrlSlugUniqueQuery query)
        {
            return ExtendableContentRepository.ExecuteQueryAsync(query);
        }

        #endregion

        #region commands
        
        public async Task<int> AddAsync(AddCustomEntityCommand command)
        {
            await ExtendableContentRepository.ExecuteCommandAsync(command);
            return command.OutputCustomEntityId;
        }

        public Task DuplicateAsync(DuplicateCustomEntityCommand command)
        {
            return ExtendableContentRepository.ExecuteCommandAsync(command);
        }

        public Task PublishAsync(PublishCustomEntityCommand command)
        {
            return ExtendableContentRepository.ExecuteCommandAsync(command);
        }

        public Task UnPublishAsync(UnPublishCustomEntityCommand command)
        {
            return ExtendableContentRepository.ExecuteCommandAsync(command);
        }
        
        public Task UpdateUrlAsync(UpdateCustomEntityUrlCommand command)
        {
            return ExtendableContentRepository.ExecuteCommandAsync(command);
        }

        public Task ReOrderAsync(ReOrderCustomEntitiesCommand command)
        {
            return ExtendableContentRepository.ExecuteCommandAsync(command);
        }

        public Task UpdateOrderingPositionAsync(UpdateCustomEntityOrderingPositionCommand command)
        {
            return ExtendableContentRepository.ExecuteCommandAsync(command);
        }

        public Task DeleteAsync(int customEntityId)
        {
            var command = new DeleteCustomEntityCommand()
            {
                CustomEntityId = customEntityId
            };

            return ExtendableContentRepository.ExecuteCommandAsync(command);
        }

        #endregion

        #region child entities

        public IAdvancedContentRepositoryCustomEntityVersionsRepository Versions()
        {
            return new ContentRepositoryCustomEntityVersionsRepository(ExtendableContentRepository);
        }

        /// <summary>
        /// Custom entity definitions are used to define the identity and
        /// behavior of a custom entity type. This includes meta data such
        /// as the name and description, but also the configuration of
        /// features such as whether the identity can contain a locale
        /// and whether versioning (i.e. auto-publish) is enabled.
        /// </summary>
        public IAdvancedContentRepositoryCustomEntityDefinitionsRepository Definitions()
        {
            return new AdvancedContentRepositoryCustomEntityDefinitionsRepository(ExtendableContentRepository);
        }

        /// <summary>
        /// Queries for working with custom entity data model schemas.
        /// </summary>
        public IAdvancedContentRepositoryCustomEntityDataModelSchemasRepository DataModelSchemas()
        {
            return new ContentRepositoryCustomEntityDataModelSchemasRepository(ExtendableContentRepository);
        }

        public IAdvancedContentRepositoryCustomEntityRoutingRulesRepository RoutingRules()
        {
            return new ContentRepositoryCustomEntityRoutingRulesRepository(ExtendableContentRepository);
        }

        public IAdvancedContentRepositoryCustomEntityByPathQueryBuilder GetByPath()
        {
            return new ContentRepositoryCustomEntityByPathQueryBuilder(ExtendableContentRepository);
        }

        #endregion
    }
}
