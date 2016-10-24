﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Cofoundry.Web.WebApi;

namespace Cofoundry.Web.Admin
{
    [AdminApiRoutePrefix("page-templates/{pageTemplateId:int}/sections")]
    public class PageTemplateSectionsApiController : BaseAdminApiController
    {
        #region private member variables
        
        private const string ID_ROUTE = "{id:int}";

        private readonly IQueryExecutor _queryExecutor;
        private readonly ApiResponseHelper _apiResponseHelper;

        #endregion

        #region constructor

        public PageTemplateSectionsApiController(
            IQueryExecutor queryExecutor,
            ApiResponseHelper apiResponseHelper
            )
        {
            _queryExecutor = queryExecutor;
            _apiResponseHelper = apiResponseHelper;
        }

        #endregion

        #region routes

        #region queries

        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> Get(int pageTemplateId)
        {

            var query = new GetPageTemplateSectionsByPageTemplateIdQuery() { PageTemplateId = pageTemplateId };
            var result = await _queryExecutor.ExecuteAsync(query);

            return _apiResponseHelper.SimpleQueryResponse(this, result);
        }

        [HttpGet]
        [Route("{pageSectionTemplateId:int}")]
        public async Task<IHttpActionResult> Get(int pageTemplateId, int pageSectionTemplateId)
        {

            var result = await _queryExecutor.GetByIdAsync<PageTemplateSectionDetails>(pageSectionTemplateId);
            return _apiResponseHelper.SimpleQueryResponse(this, result);
        }

        #endregion

        #region commands

        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> Post(AddPageTemplateSectionCommand command)
        {
            return await _apiResponseHelper.RunCommandAsync(this, command);
        }

        [HttpPatch]
        [Route(ID_ROUTE)]
        public async Task<IHttpActionResult> Patch(int id, Delta<UpdatePageTemplateSectionCommand> delta)
        {
            return await _apiResponseHelper.RunCommandAsync(this, id, delta);
        }

        [HttpDelete]
        [Route(ID_ROUTE)]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var command = new DeletePageTemplateSectionCommand();
            command.PageTemplateSectionId = id;

            return await _apiResponseHelper.RunCommandAsync(this, command);
        }

        [HttpPut]
        [Route(ID_ROUTE + "/UpdateModuleTypes")]
        public async Task<IHttpActionResult> UpdateModuleTypes(int pageTemplateId, int id, UpdatePageTemplateSectionModuleTypesCommand command)
        {
            return await _apiResponseHelper.RunCommandAsync(this, command);
        }

        #endregion

        #endregion
    }
}