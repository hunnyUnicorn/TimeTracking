using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private string connString;
        private bool _disposed;

        private ISecurityRepository securityRepository;
        private ISupervisorRepository supervisorRepository;
        private IGeneralRepository generalRepository;
        private IDeveloperRepository developerRepository;
        private IClientsRepository clientsRepository;
        private IMaintenanceRepository maintenanceRepository;


        public UnitOfWork(string connectionString)
        {
            connString = connectionString;
        }


        public ISecurityRepository SecurityRepository
        {
            get { return securityRepository ?? (securityRepository = new SecurityRepository(connString)); }
        }

        public ISupervisorRepository SupervisorRepository
        {
            get { return supervisorRepository ?? (supervisorRepository = new SupervisorRepository(connString)); }
        }

        public IGeneralRepository GeneralRepository
        {
            get { return generalRepository ?? (generalRepository = new GeneralRepository(connString)); }
        }
        public IDeveloperRepository DeveloperRepository
        {
            get { return developerRepository ?? (developerRepository = new DeveloperRepository(connString)); }
        }
        public IClientsRepository ClientsRepository
        {
            get { return clientsRepository ?? (clientsRepository = new ClientsRepository(connString)); }
        }
        public IMaintenanceRepository MaintenanceRepository
        {
            get { return maintenanceRepository ?? (maintenanceRepository = new MaintenanceRepository(connString)); }
        }
        public void Reset()
        {
            securityRepository = null;
            supervisorRepository = null;
            generalRepository = null;
            developerRepository = null;
            clientsRepository = null;
            maintenanceRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
