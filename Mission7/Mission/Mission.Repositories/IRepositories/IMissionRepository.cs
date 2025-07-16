using Mission.Entities.Models.CommonModel;
using Mission.Entities.Models.MissionsModels;

namespace Mission.Repositories.IRepositories
{
    public interface IMissionRepository
    {
        List<MissionResponseModel> MissionList();
        string AddMission(AddMissionRequestModel request);
        MissionResponseModel GetMissionById(int missionId);
        string DeleteMissionById(int missionId);

        List<MissionResponseModel> ClientMissionList(int userId);
        List<DropDownResponseModel> GetMissionThemeList();
        List<DropDownResponseModel> GetMissionSkillList();
        string ApplyMission(ApplyMissionRequestModel request);
        string ApproveMission(int id);
        string UploadImage(UploadFileRequestModel upload);
        string DeleteImage(int id);
        string DeleteMissionApplication(int id);
        List<MissionApplicationResponseModel> MissionApplicationList();
        
    }
}
