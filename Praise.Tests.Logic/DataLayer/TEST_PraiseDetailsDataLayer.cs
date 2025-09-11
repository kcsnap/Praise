using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Praise.Api.DataLayer;
using Praise.Api.Logic;
using Praise.Models;
using Praise.Models.Configuration;
using Praise.WebApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praise.Tests.Logic.DataLayer
{
    [TestClass]
    public sealed class TEST_PraiseDetailsDataLayer
    {
        //[TestMethod]
        //public async Task WHENSave_THENCanReadBack()
        //{
        //    var options = Options.Create(new DatabaseOptions());
        //    PraiseDetailsDataLayer_ForTesting praiseDB = new PraiseDetailsDataLayer_ForTesting(options);
            
        //    var praiseDTO = new PraiseDetails
        //    {
        //        id = Guid.NewGuid(),
        //        FromEmail = "testFrom",
        //        ToEmail = "testTo",
        //        PraiseType = PraiseTypeEnum.Saviour,
        //        PraiseDate = DateTime.Now,
        //        ImageUrl = "img url",
        //        Text = PraiseTypeEnum.Saviour.GetDescription()
        //    };

        //    var result = await praiseDB.Save(praiseDTO);
        //    Assert.AreEqual(result.id, praiseDTO.id);

        //    // tidy up
        //    await praiseDB.Delete(praiseDTO.id.Value);
        //}

        //[TestMethod]
        //public async Task WHENSaveMultiple_THENCanReadAllBack()
        //{
        //    var options = Options.Create(new DatabaseOptions());
        //    PraiseDetailsDataLayer_ForTesting praiseDB = new PraiseDetailsDataLayer_ForTesting(options);

        //    var praise1 = Guid.NewGuid();
        //    var praise2 = Guid.NewGuid();
        //    var praise3 = Guid.NewGuid();
        //    var praise4 = Guid.NewGuid();

        //    var praiseDTO = new PraiseDetails
        //    {
        //        id = praise1,
        //        FromEmail = "testFrom",
        //        ToEmail = "testTo",
        //        PraiseType = PraiseTypeEnum.Saviour
        //    };

        //    await praiseDB.Save(praiseDTO);
        //    praiseDTO.id = praise2;
        //    await praiseDB.Save(praiseDTO);
        //    praiseDTO.id = praise3;
        //    praiseDTO.ToEmail = "testTo2";
        //    await praiseDB.Save(praiseDTO);
        //    praiseDTO.id = praise4;
        //    praiseDTO.ToEmail = "testTo2";
        //    await praiseDB.Save(praiseDTO);

        //    var results = await praiseDB.ReadByPraiseSenderEmail("testFrom", 1, 10);
        //    Assert.AreEqual(4, results.Count());

        //    var results2 = await praiseDB.ReadByPraiseReceiverEmail("testTo", 1, 10);
        //    Assert.AreEqual(2, results2.Count());

        //    // tidy up
        //    await praiseDB.Delete(praise1);
        //    await praiseDB.Delete(praise2);
        //    await praiseDB.Delete(praise3);
        //    await praiseDB.Delete(praise4);
        //}
    }

    public class PraiseDetailsDataLayer_ForTesting : PraiseDetailsDataLayer
    {
        public PraiseDetailsDataLayer_ForTesting(IOptions<DatabaseOptions> dbOptions, ILogger<PraiseDetailsDataLayer_ForTesting> logger) : base(dbOptions, logger)
        {
        }

        public async Task<PraiseDetails> Delete(Guid id)
        {
            try
            {
            ItemResponse<PraiseDetails> response = await praiseContainer.DeleteItemAsync<PraiseDetails>(
                    id: id.ToString(),
                    partitionKey: new PartitionKey(id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                // todo - logging
                ex = ex;
                throw new Exception("CosmosDB error when saving PraiseDetails", ex);
            }
        }
    }
}
