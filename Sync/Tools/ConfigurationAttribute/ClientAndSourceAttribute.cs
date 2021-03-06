﻿using Sync.Client;
using Sync.Plugins;
using Sync.Tools.ConfigurationAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Tools.ConfigurationAttribute
{
    class ClientListAttribute : ListAttribute
    {
        public override string[] ValueList => ClientManager.Instance.Clients.Select(c=>c.ClientName).ToArray();
    }

    class SourceListAttribute : ListAttribute
    {
        public override string[] ValueList => SyncHost.Instance.Sources.SourceList.Select(s => s.Name).ToArray();
    }
}
