using devops_metrics.Entities;
using MongoDB.Driver;

namespace devops_metrics.Services;

public class InfoService
{
   public async Task<Dictionary<string, Dictionary<string, HashSet<string>>>> GetInfo()
   {
      
      var changes = await new ChangeService().GetChanges(-1200, null, null, null);
      Dictionary<string, Dictionary<string, HashSet<string>>> organizationsWithProjectAndRepositories = new Dictionary<string, Dictionary<string, HashSet<string>>>(); 
      changes.ForEach(change =>
      {
         try
         {
            organizationsWithProjectAndRepositories.TryAdd(change.Organization, new Dictionary<string, HashSet<string>>());
         }
         catch (Exception e) {}
         try
         {
            organizationsWithProjectAndRepositories[change.Organization].TryAdd(change.Project, new HashSet<string>());
         }
         catch (Exception e) {}

         try
         {
            organizationsWithProjectAndRepositories[change.Organization][change.Project].Add(change.Repository);
         }
         catch (Exception e) {}
      });
      return organizationsWithProjectAndRepositories;
   }
}