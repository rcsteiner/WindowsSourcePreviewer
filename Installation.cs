// Stephen Toub

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MsdnMag
{
	[RunInstaller(true)]
	public class ComInstaller : Installer
	{
        public override void Install(IDictionary stateSaver)
        {
            try
            {
                base.Install(stateSaver);
                RegistrationServices regsrv = new RegistrationServices();
                if (!regsrv.RegisterAssembly(GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase))
                {
                    throw new InstallException("Failed to register for COM interop.");
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.ToString());
                throw;
            }
        }

		public override void Uninstall(IDictionary savedState)
		{
            base.Uninstall(savedState);
			RegistrationServices regsrv = new RegistrationServices();
			if (!regsrv.UnregisterAssembly(GetType().Assembly))
			{
                throw new InstallException("Failed to unregister for COM interop.");
			}
        }
	}
}