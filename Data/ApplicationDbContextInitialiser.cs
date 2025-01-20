//using system;
//using system.collections.generic;
//using system.linq;
//using system.security.claims;
//using system.text;
//using system.threading.tasks;
//using microsoft.aspnetcore.identity;
//using microsoft.entityframeworkcore;
//using microsoft.extensions.logging;

//namespace data
//{
//    public class applicationdbcontextinitialiser
//    {
//        private readonly ilogger<applicationdbcontextinitialiser> _logger;
//        private readonly applicationdbcontext _context;
//        private readonly usermanager<applicationuser> _usermanager;
//        private readonly rolemanager<identityrole> _rolemanager;

//        public applicationdbcontextinitialiser(ilogger<applicationdbcontextinitialiser> logger, applicationdbcontext context, usermanager<applicationuser> usermanager, rolemanager<identityrole> rolemanager)
//        {
//            _logger = logger;
//            _context = context;
//            _usermanager = usermanager;
//            _rolemanager = rolemanager;
//        }
//        public async task initialiseasync()
//        {
//            try
//            {
//                if (_context.database.issqlserver())
//                {
//                    await _context.database.migrateasync();
//                }
//            }
//            catch (exception ex)
//            {
//                _logger.logerror(ex, "an error occurred while initialising the database.");
//                throw;
//            }
//        }
//        public async task seedasync()
//        {
//            try
//            {
//                await tryseedasync();
//            }
//            catch (exception ex)
//            {
//                _logger.logerror(ex, "an error occurred while seeding the database.");
//                throw;
//            }
//        }

//        public async task tryseedasync()
//        {
//            // default roles
//            var administratorrole = new identityrole("administrator");

//            if (_rolemanager.roles.all(r => r.name != administratorrole.name))
//            {
//                var role = await _rolemanager.createasync(administratorrole);
//                if (role != null)
//                {
//                    await _rolemanager.addclaimasync(administratorrole, new claim("roleclaim", "hasroleview"));
//                    await _rolemanager.addclaimasync(administratorrole, new claim("roleclaim", "hasroleadd"));
//                    await _rolemanager.addclaimasync(administratorrole, new claim("roleclaim", "hasroleedit"));
//                    await _rolemanager.addclaimasync(administratorrole, new claim("roleclaim", "hasroledelete"));
//                }
//            }

//            // default users
//            var administrator = new applicationuser { username = "unifiedappadmin", email = "unifiedappadmin" };

//            if (_usermanager.users.all(u => u.username != administrator.username))
//            {
//                await _usermanager.createasync(administrator, "unifiedappadmin1!");
//                if (!string.isnullorwhitespace(administratorrole.name))
//                {
//                    await _usermanager.addtorolesasync(administrator, new[] { administratorrole.name });
//                }
//            }
//        }
//    }
//}
