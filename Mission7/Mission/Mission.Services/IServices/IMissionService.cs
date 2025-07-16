using Mission.Entities.Models.CommonModel;
using Mission.Entities.Models.MissionsModels;

namespace Mission.Services.IServices
{
    public interface IMissionService
    {
        List<MissionResponseModel> GetMissionList();
        MissionResponseModel GetMissionById(int missionId);
        string DeleteMission(int missionId);
        string AddMission(AddMissionRequestModel request);
        List<MissionResponseModel> ClientMissionList(int userId);
        List<DropDownResponseModel> GetMissionThemeList();
        List<DropDownResponseModel> GetMissionSkillList();
        string ApplyMission(ApplyMissionRequestModel request);
        string ApproveMission(int id);
        string UploadImage(UploadFileRequestModel upload);
        string DeleteImage(int id);
        string DeleteMissionApplication(int id);
        List<MissionApplicationResponseModel> MissionApplicationList();

        List<MissionResponseModel> MissionList();

    }
}
