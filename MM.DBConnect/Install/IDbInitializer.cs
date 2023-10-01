using Pspcl.Core.Domain;

namespace Pspcl.DBConnect.Install
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        Task Initialize();

        /// <summary>
        /// 
        /// </summary>
        Task CreateRoles();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<User> CreateDefaultAdminUser();

        //for Seeding MasterData
        Task CreateDefaultMaterialGroup();
        Task CreateDefaultMaterialType();
        Task CreateDefaultRating();
        Task CreateDefaultRatingMaterialTypeMapping();
        Task CreateDefaultMaterial();
        Task CreateDefaultCircle();
        Task CreateDefaultDivision();
        Task CreateDefaultSubDivision();


    }
}
