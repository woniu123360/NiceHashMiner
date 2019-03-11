﻿using System;
using System.Diagnostics;
using NiceHashMiner.Devices.Algorithms;
using NiceHashMinerLegacy.Common.Enums;

namespace NiceHashMiner.Devices
{
    public class CpuComputeDevice : ComputeDevice
    {
        private readonly PerformanceCounter _cpuCounter;

        public override float Load
        {
            get
            {
                try
                {
                    if (_cpuCounter != null) return _cpuCounter.NextValue();
                }
                catch (Exception e) { Helpers.ConsolePrint("CPUDIAG", e.ToString()); }
                return -1;
            }
        }

        public CpuComputeDevice(int id, string group, string name, int threads, ulong affinityMask, int cpuCount)
            : base(id,
                name,
                true,
                DeviceGroupType.CPU,
                DeviceType.CPU,
                string.Format(Translations.Tr("CPU#{0}"), cpuCount),
                0)
        {
            Threads = threads;
            AffinityMask = affinityMask;
            Uuid = GetUuid(ID, GroupNames.GetGroupName(DeviceGroupType, ID), Name, DeviceGroupType);
            AlgorithmSettings = DefaultAlgorithms.GetAlgorithmsForDevice(this);
            Index = ID; // Don't increment for CPU

            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
        }
    }
}
