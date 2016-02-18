//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.Windows.Forms;
//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Smo.SqlEnum;
//using System.Collections.Specialized;
//using System.Data.SqlClient;
//using Microsoft.Synchronization;
//using Microsoft.Synchronization.Data;
//using Microsoft.Synchronization.Data.SqlServer;
//namespace Hotel_POS
//{
//    /// <summary>
//    /// Interaction logic for BackUp_RestoreData.xaml
//    /// </summary>
//    public partial class BackUp_RestoreData : Window
//    {
//        int Role_ID;

//        public BackUp_RestoreData()
//        {
//            InitializeComponent();
//        }
//        Server srv;
//        ServerConnection srvConn;
//        //my SQL user doesnt have sufficient permissions,
//        //so i am using my windows account

//        /// <summary>

//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        #region Events


//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {
//            try
//            {
//                Common.Initialise();

//                Role_ID = Convert.ToInt32(((System.Windows.Controls.Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());

//                srvConn = new ServerConnection(Common.ServerName, Common.UserId, Common.Password);
//                srv = new Server(srvConn);
//            }
//            catch (Exception EX)
//            {

//                System.Windows.MessageBox.Show(EX.Message);
//            }
//        }

//        private void button1_Click(object sender, RoutedEventArgs e)
//        {
//            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
//            folderDialog.SelectedPath = "C:\\";
//            DialogResult result = folderDialog.ShowDialog();
//            if (result.ToString() == "OK")
//                txtFolderPath.Text = folderDialog.SelectedPath + "\\" + DateTime.Now.ToString("dd-MM-yyyy-HH_MM_ss") + ".bak";

//        }

//        private void btnBrowse_Click(object sender, RoutedEventArgs e)
//        {
//            OpenFileDialog dialog = new OpenFileDialog();
//            dialog.InitialDirectory = "C:\\";
//            dialog.Filter = "Back up Files (*.bak)|*.txt|All files (*.*)|*.*";

//            DialogResult result = dialog.ShowDialog();
//            if (result.ToString() == "OK")
//                txtBackupFilePath.Text = dialog.FileName;
//        }



//        private void btnBackupData_Click(object sender, RoutedEventArgs e)
//        {
//            try
//            {


//                if (txtFolderPath.Text != "")
//                    BackUpDB(Common.Live_DBName);
//            }
//            catch (Exception ex)
//            {
//                System.Windows.MessageBox.Show(ex.Message);
//            }
//        }

//        static void Program_ApplyChangeFailed(object sender, DbApplyChangeFailedEventArgs e)
//        {
//            // display conflict type
//            Console.WriteLine(e.Conflict.Type);

//            // display error message 
//            Console.WriteLine(e.Error);
//        }
//        #endregion



//        #region Methods
//        public void RestoreDB(string backUpFilePath, string databaseName)
//        {
//            try
//            {
//                Restore restore = new Restore();
//                //Set type of backup to be performed to database
//                restore.Database = databaseName;
//                restore.Action = RestoreActionType.Database;

//                // Replace the Db if already exists
//                restore.ReplaceDatabase = true;

//                //Set up the backup device to use filesystem.         
//                restore.Devices.AddDevice(backUpFilePath, DeviceType.File);
//                //set ReplaceDatabase = true to create new database
//                //regardless of the existence of specified database
//                restore.ReplaceDatabase = true;
//                //If you have a differential or log restore to be followed,
//                //you would specify NoRecovery = true
//                restore.NoRecovery = false;
//                Server sqlServer = new Server(srvConn);
//                //SqlRestore method starts to restore database           
//                // Added By naved Start
//                SqlConnection.ClearAllPools();
//                //Added By naved end
//                sqlServer.KillAllProcesses(databaseName);
//                restore.SqlRestore(sqlServer);

//                SyncDB();
//                System.Windows.Forms.MessageBox.Show("Restore operation succeeded");

//            }
//            catch (Exception ex)
//            {
//                System.Windows.Forms.MessageBox.Show("Restore operation failed");
//                System.Windows.Forms.MessageBox.Show(ex.Message);
//            }
//            Console.ReadLine();
//        }
//        public void BackUpDB(string databaseName)
//        {
//            try
//            {


//                // If there was a SQL connection created
//                // Create a new backup operation
//                Backup bkpDatabase = new Backup();
//                // Set the backup type to a database backup
//                bkpDatabase.Action = BackupActionType.Database;
//                // Set the database that we want to perform a backup on
//                bkpDatabase.Database = databaseName;

//                // Set the backup device to a file
//                BackupDeviceItem bkpDevice = new BackupDeviceItem(txtFolderPath.Text,
//                       DeviceType.File);


//                // Add the backup device to the backup
//                bkpDatabase.Devices.Add(bkpDevice);
//                // Perform the backup

//                bkpDatabase.SqlBackup(srv);

//                System.Windows.Forms.MessageBox.Show("Back up operation succeeded");
//            }
//            catch (Exception ex)
//            {

//                System.Windows.Forms.MessageBox.Show("Back up operation failed");
//                System.Windows.Forms.MessageBox.Show(ex.Message);
//            }
//        }
//        void SyncDB()
//        {
//            try
//            {

//                string[] ArrTableNames = { "Card_Details", "Card_Ledger", "Card_Master", "Card_Status_Master", "Employee_Master", "Grade_Master", "Item_Group_Master", "Item_Master" ,
//                                         "Role_Master","Transaction_Details","Transaction_Master","User_Master","Company_Master"
//                                         };


//                SqlConnection serverConn = new SqlConnection(Common.Live_ConnString);
//                SqlConnection clientConn = new System.Data.SqlClient.SqlConnection(Common.Demo_ConnString);
//                //Commented By Naved 
//                SqlSyncScopeDeprovisioning ServerSqlDepro = new SqlSyncScopeDeprovisioning(serverConn);
//                //ServerSqlDepro.ScriptDeprovisionScope("MySyncScope");
//                ServerSqlDepro.DeprovisionStore();
//                SqlSyncScopeDeprovisioning clientSqlDepro = new SqlSyncScopeDeprovisioning(clientConn);
//                // clientSqlDepro.ScriptDeprovisionScope("MySyncScope");
//                clientSqlDepro.DeprovisionStore();

//                //ReProvision(serverConn, "MySyncScope", ArrTableNames);
//                //Comment End
//                //  ReProvision(serverConn, "MySyncScope", ArrTableNames);


//                DbSyncScopeDescription scopeDesc = new DbSyncScopeDescription("MySyncScope");
//                // get the description of the CUSTOMER & PRODUCT table from SERVER database


//                List<DbSyncTableDescription> lst = new List<DbSyncTableDescription>();
//                DbSyncTableDescription objTable;
//                foreach (var item in ArrTableNames)
//                {
//                    objTable = SqlSyncDescriptionBuilder.GetDescriptionForTable(item, serverConn);
//                    lst.Add(objTable);
//                    scopeDesc.Tables.Add(objTable);
//                }



//                // create a server scope provisioning object based on the MySyncScope
//                SqlSyncScopeProvisioning serverProvision = new SqlSyncScopeProvisioning(serverConn, scopeDesc);

//                // skipping the creation of table since table already exists on server
//                serverProvision.SetCreateTableDefault(DbSyncCreationOption.Skip);

//                if (!serverProvision.ScopeExists("MySyncScope"))
//                // start the provisioning process
//                {
//                    serverProvision.Apply();

//                    Console.WriteLine("Server Successfully Provisioned.");

//                }
//                else
//                {
//                    Console.WriteLine("Already Server Provisioned.");

//                }
//                //// get the description of SyncScope from the server database
//                //DbSyncScopeDescription scopeDesc = SqlSyncDescriptionBuilder.GetDescriptionForScope("MySyncScope", serverConn);

//                // create server provisioning object based on the SyncScope
//                SqlSyncScopeProvisioning clientProvision = new SqlSyncScopeProvisioning(clientConn, scopeDesc);

//                if (!clientProvision.ScopeExists("MySyncScope"))
//                {
//                    // starts the provisioning process
//                    clientProvision.Apply();
//                }

//                // create the sync orhcestrator
//                SyncOrchestrator syncOrchestrator = new SyncOrchestrator();

//                // set local provider of orchestrator to a sync provider associated with the 
//                // MySyncScope in the client database
//                syncOrchestrator.LocalProvider = new SqlSyncProvider("MySyncScope", clientConn);

//                // set the remote provider of orchestrator to a server sync provider associated with
//                // the MySyncScope in the server database
//                syncOrchestrator.RemoteProvider = new SqlSyncProvider("MySyncScope", serverConn);

//                // set the direction of sync session to Upload and Download
//                syncOrchestrator.Direction = SyncDirectionOrder.UploadAndDownload;

//                // subscribe for errors that occur when applying changes to the client
//                ((SqlSyncProvider)syncOrchestrator.LocalProvider).ApplyChangeFailed += new EventHandler<DbApplyChangeFailedEventArgs>(Program_ApplyChangeFailed);

//                // execute the synchronization process
//                SyncOperationStatistics syncStats = syncOrchestrator.Synchronize();

//                // print statistics
//                Console.WriteLine("Start Time: " + syncStats.SyncStartTime);
//                Console.WriteLine("Total Changes Uploaded: " + syncStats.UploadChangesTotal);
//                Console.WriteLine("Total Changes Downloaded: " + syncStats.DownloadChangesTotal);
//                Console.WriteLine("Complete Time: " + syncStats.SyncEndTime);
//                Console.WriteLine(String.Empty);

//            }
//            catch (Exception ex)
//            {
//                System.Windows.Forms.MessageBox.Show("Sync operation failed");
//                System.Windows.Forms.MessageBox.Show(ex.Message);
//                return;

//            }
//        }

//        private void btnRestore_Click(object sender, RoutedEventArgs e)
//        {
//            if (txtBackupFilePath.Text != "")
//                RestoreDB(txtBackupFilePath.Text, Common.Demo_DBName);
//        }

//        #endregion


//        public void DropTrackingForTable(SqlConnection conn, string tableName)
//        {
//            SqlCommand comm;
//            StringBuilder sb = new StringBuilder();
//            //Drop tracking table & triggers
//            sb.AppendFormat(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}_tracking]') AND type in (N'U'))
//                DROP TABLE [dbo].[{0}_tracking]
//                IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[{0}_insert_trigger]'))
//                DROP TRIGGER [dbo].[{0}_insert_trigger]
//                IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[{0}_delete_trigger]'))
//                DROP TRIGGER [dbo].[{0}_delete_trigger]
//                IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[{0}_update_trigger]'))
//                DROP TRIGGER [dbo].[{0}_update_trigger]", tableName);
//            //Drop Procedures
//            foreach (string procName in new string[] { "delete", "deletemetadata", "insert", "insertmetadata", "update", "updatemetadata", "selectrow", "selectchanges" })
//            {
//                sb.AppendFormat(@"
//                  IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}_{1}]') AND type in (N'P', N'PC'))
//                  DROP PROCEDURE [dbo].[{0}_{1}]
//                  ", tableName, procName);
//            }
//            using (comm = new SqlCommand(sb.ToString(), conn))
//            {
//                conn.Open();
//                comm.ExecuteNonQuery();
//                conn.Close();
//            }
//        }

//        public void ReProvision(SqlConnection conn, string scopeName, IEnumerable<string> tablesThatChanged)
//        {
//            var serverConfig = new SqlSyncScopeProvisioning();
//            var scopeDesc = SqlSyncDescriptionBuilder.GetDescriptionForScope(scopeName, conn);
//            scopeDesc.ScopeName += "_temp";

//            foreach (var tableName in tablesThatChanged)
//            {
//                DropTrackingForTable(conn, tableName);

//                var bracketedName = string.Format("[{0}]", tableName);
//                scopeDesc.Tables.Remove(scopeDesc.Tables[bracketedName]);

//                var tableDescription = SqlSyncDescriptionBuilder.GetDescriptionForTable(bracketedName, conn);
//                scopeDesc.Tables.Add(tableDescription);
//            }

//            serverConfig.PopulateFromScopeDescription(scopeDesc);
//            serverConfig.SetCreateProceduresDefault(DbSyncCreationOption.Skip);
//            serverConfig.SetCreateTableDefault(DbSyncCreationOption.Skip);
//            serverConfig.SetCreateTrackingTableDefault(DbSyncCreationOption.Skip);
//            serverConfig.SetCreateTriggersDefault(DbSyncCreationOption.Skip);
//            serverConfig.SetPopulateTrackingTableDefault(DbSyncCreationOption.Skip);
//            serverConfig.SetCreateProceduresForAdditionalScopeDefault(DbSyncCreationOption.Skip);

//            foreach (var tableName in tablesThatChanged)
//            {
//                var bracketedName = string.Format("[{0}]", tableName);

//                serverConfig.Tables[bracketedName].CreateProcedures = DbSyncCreationOption.Create;
//                serverConfig.Tables[bracketedName].CreateTrackingTable = DbSyncCreationOption.Create;
//                serverConfig.Tables[bracketedName].PopulateTrackingTable = DbSyncCreationOption.Create;
//                serverConfig.Tables[bracketedName].CreateTriggers = DbSyncCreationOption.Create;
//            }
//            serverConfig.Apply(conn);

//            using (SqlCommand comm1 = new SqlCommand(@"declare @config_id uniqueidentifier, @config_data xml  
//         SELECT @config_id=sc.config_id, @config_data=sc.config_data  
//         From scope_config sc Join [scope_info] si on si.scope_config_id=sc.config_id  
//         WHERE si.scope_name = @scope_name + '_temp'  
//   
//         Update [scope_config] Set config_data=@config_data  
//         From scope_config sc Join [scope_info] si on si.scope_config_id=sc.config_id  
//         WHERE si.scope_name = @scope_name  
//   
//         Delete [scope_config] WHERE config_id=@config_id;  
//         Delete [scope_info] WHERE scope_config_id=@config_id;", conn))
//            {
//                conn.Open();
//                comm1.Parameters.AddWithValue("@scope_name", scopeName);
//                comm1.ExecuteNonQuery();
//                conn.Close();
//            }
//        }


//    }
//}
