﻿using Cofoundry.Domain.Extendable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Domain
{
    public class ContentRepositoryRewriteRuleGetAllQueryBuilder
        : IContentRepositoryRewriteRuleGetAllQueryBuilder
        , IExtendableContentRepositoryPart
    {
        public ContentRepositoryRewriteRuleGetAllQueryBuilder(
            IExtendableContentRepository contentRepository
            )
        {
            ExtendableContentRepository = contentRepository;
        }

        public IExtendableContentRepository ExtendableContentRepository { get; }

        public Task<ICollection<RewriteRuleSummary>> AsSummariesAsync()
        {
            var query = new GetAllRewriteRuleSummariesQuery();
            return ExtendableContentRepository.ExecuteQueryAsync(query);
        }
    }
}
