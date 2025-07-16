using Microsoft.EntityFrameworkCore;
using Mission.Entities.context;
using Mission.Entities.Entities;
using Mission.Entities.Models.CommonModel;
using Mission.Entities.Models.MissionsModels;
using Mission.Repositories.IRepositories;

namespace Mission.Repositories.Repositories
{
    public class MissionRepository(MissionDbContext dbContext) : IMissionRepository
    {
        private readonly MissionDbContext _dbContext = dbContext;

        public List<MissionResponseModel> ClientMissionList(int userId)
        {
            var missions = _dbContext.Missions
                            .Where(x => !x.IsDeleted)
                            .OrderBy(x => x.StartDate)
                            .Select(x => new MissionResponseModel()
                            {
                                Id = x.Id,
                                CityId = x.CityId,
                                CityName = x.City.CityName,
                                CountryId = x.CountryId,
                                CountryName = x.Country.CountryName,
                                EndDate = x.EndDate,
                                MissionDescription = x.MissionDescription,
                                MissionImages = x.MissionImages,
                                MissionOrganisationDetail = x.MissionOrganisationDetail,
                                MissionOrganisationName = x.MissionOrganisationName,
                                MissionSkillId = x.MissionSkillId,
                                MissionThemeId = x.MissionThemeId,
                                MissionThemeName = x.MissionTheme.ThemeName,
                                MissionTitle = x.MissionTitle,
                                MissionType = x.MissionType,
                                StartDate = x.StartDate,
                                TotalSheets = x.TotalSheets,
                                MissionSkillName = string.Join(",", _dbContext.MissionSkill
                                    .Where(ms => x.MissionSkillId.Contains(ms.Id.ToString()))
                                    .Select(ms => ms.SkillName))
                                .ToList(),
                                MissionApplyStatus = _dbContext.MissionApplications.Any(ma => ma.UserId == userId && ma.MissionId == x.Id) ? "Applied" : "Apply",
                            }).ToList();
            return missions;
        }

        public List<MissionResponseModel> MissionList()
        {
            var missions = _dbContext.Missions.Where(x => !x.IsDeleted)
                .Select(x => new MissionResponseModel()
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    CityName = x.City.CityName,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    EndDate = x.EndDate,
                    MissionDescription = x.MissionDescription,
                    MissionImages = x.MissionImages,
                    MissionOrganisationDetail = x.MissionOrganisationDetail,
                    MissionOrganisationName = x.MissionOrganisationName,
                    MissionSkillId = x.MissionSkillId,
                    MissionThemeId = x.MissionThemeId,
                    MissionThemeName = x.MissionTheme.ThemeName,
                    MissionTitle = x.MissionTitle,
                    MissionType = x.MissionType,
                    StartDate = x.StartDate,
                    TotalSheets = x.TotalSheets
                }).ToList();
            return missions;
        }

        public string AddMission(AddMissionRequestModel request)
        {

            var exists = _dbContext.Missions.Any(x => x.MissionTitle.ToLower() == request.MissionTitle.ToLower()
                                                        && x.CityId == request.CityId
                                                        && x.StartDate.Date == request.StartDate.Date
                                                        && x.EndDate.Date == request.EndDate.Date && !x.IsDeleted);
            if (exists)
            {
                throw new Exception("Mission already exist");
            }

            var mission = new Missions()
            {
                MissionDescription = request.MissionDescription,
                MissionImages = request.MissionImages,
                CityId = request.CityId,
                CountryId = request.CountryId,
                MissionOrganisationDetail = request.MissionOrganisationDetail,
                MissionOrganisationName = request.MissionOrganisationName,
                MissionSkillId = request.MissionSkillId,
                MissionThemeId = request.MissionThemeId,
                MissionTitle = request.MissionTitle,
                MissionType = request.MissionType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedDate = DateTime.UtcNow,
                TotalSheets = request.TotalSheets,
                IsDeleted = false
            };

            _dbContext.Missions.Add(mission);
            _dbContext.SaveChanges();
            return "Mission Save Successfully";
        }

        public MissionResponseModel GetMissionById(int missionId)
        {
            return _dbContext.Missions
                .Where(x => x.Id == missionId && !x.IsDeleted)
                .Select(x => new MissionResponseModel
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    CityName = x.City.CityName,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    EndDate = x.EndDate,
                    MissionDescription = x.MissionDescription,
                    MissionImages = x.MissionImages,
                    MissionOrganisationDetail = x.MissionOrganisationDetail,
                    MissionOrganisationName = x.MissionOrganisationName,
                    MissionSkillId = x.MissionSkillId,
                    MissionThemeId = x.MissionThemeId,
                    MissionThemeName = x.MissionTheme.ThemeName,
                    MissionTitle = x.MissionTitle,
                    MissionType = x.MissionType,
                    StartDate = x.StartDate,
                    TotalSheets = x.TotalSheets
                }).FirstOrDefault() ?? throw new Exception("Mission not found");
        }

        public string DeleteMissionById(int missionId)
        {
            var mission = _dbContext.Missions.Where(x => x.Id == missionId && !x.IsDeleted).ExecuteUpdate(x => x.SetProperty(p => p.IsDeleted, true));
            return "Mission deleted successfully";
        }

        public string AppyMission(ApplyMissionRequestModel request)
        {
            var mission = _dbContext.Missions.Where(x => x.Id == request.MissionId && !x.IsDeleted).FirstOrDefault();
            if (mission == null)
            {
                throw new Exception("Mission not found");
            }
            if (mission.TotalSheets == 0)
            {
                throw new Exception("Mission is full");
            }
            if (mission.TotalSheets < request.Sheet)
            {
                throw new Exception("Not Available seats");
            }
            var missionApplication = new MissionApplication()
            {
                MissionId = request.MissionId,
                UserId = request.UserId,
                AppliedDate = request.AppliedDate,
                Status = request.Status,
                Sheet = request.Sheet,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };
            _dbContext.MissionApplications.Add(missionApplication);
            _dbContext.SaveChanges();
            mission.TotalSheets -= request.Sheet;

            _dbContext.Missions.Update(mission);
            _dbContext.SaveChanges();
            return "Mission applied successfully";
        }

        public string ApproveMission(int id)
        {
            var exists = _dbContext.MissionApplications.Any(x => x.Id == id);

            if (!exists)
            {
                throw new Exception("Mission application not found");
            }
            var updateCount = _dbContext.MissionApplications
                .Where(x => x.Id == id)
                .ExecuteUpdate(m => m.SetProperty(p => p.Status, true));

            return updateCount > 0 ? "Mission application approved successfully" : "Failed to approve mission application";
        }

        public List<DropDownResponseModel> GetMissionThemeList()
        {
            throw new NotImplementedException();
        }

        public List<DropDownResponseModel> GetMissionSkillList()
        {
            throw new NotImplementedException();
        }

        public string ApplyMission(ApplyMissionRequestModel request)
        {
            throw new NotImplementedException();
        }
        public string DeleteMission(int missionId)
        {
            return DeleteMissionById(missionId);
        }
        public List<MissionResponseModel> GetMissionList()
        {
            return MissionList();
        }
        public List<MissionApplicationResponseModel> MissionApplicationList()
        {
            return _dbContext.MissionApplications
                .Where(x => !x.IsDeleted && !x.Mission.IsDeleted && !x.User.IsDeleted)
                .Select(x => new MissionApplicationResponseModel()
                {
                    Id = x.Id,
                    AppliedDate = x.AppliedDate,
                    MissionId = x.MissionId,
                    MissionTitle = x.Mission.MissionTitle,
                    MissionTheme = x.Mission.MissionTheme.ThemeName,
                    Sheets = x.Sheet,
                    Status = x.Status,
                    UserId = x.User.Id,
                    UserName = $"{x.User.FirstName} {x.User.LastName}"
                }).ToList();

        }
        public string DeleteMissionApplication(int Id)
        {
            var mission = _dbContext.MissionApplications
                .Where(x => x.Id == Id && !x.IsDeleted)
                .ExecuteUpdate(x => x.SetProperty(p => p.IsDeleted, true));
            return"Mission application deleted successfully";
        }

        public string UploadImage(UploadFileRequestModel upload)
        {
            throw new NotImplementedException();
        }

        public string DeleteImage(int id)
        {
            throw new NotImplementedException();
        }
    }
    }
