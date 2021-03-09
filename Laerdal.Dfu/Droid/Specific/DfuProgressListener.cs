using Android.App;
using Laerdal.Dfu.Enums;

namespace Laerdal.Dfu.Specific
{
    public class DfuProgressListener : Laerdal.Dfu.Droid.DfuProgressListenerAdapter
    {
        private DfuInstallation DfuInstallation { get; }

        public DfuProgressListener(DfuInstallation dfuInstallation)
        {
            DfuInstallation = dfuInstallation;
            Laerdal.Dfu.Droid.DfuServiceListenerHelper.RegisterProgressListener(Application.Context, this);
        }

        protected override void Dispose(bool disposing)
        {
            Laerdal.Dfu.Droid.DfuServiceListenerHelper.UnregisterProgressListener(Application.Context, this);
            base.Dispose(disposing);
        }

        public override void OnError(string deviceAddress,
                                     int error,
                                     int errorType,
                                     string message)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.OnDfuError((DfuError) error, message);
        }

        public override void OnDeviceConnected(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Connected;
        }

        public override void OnDeviceConnecting(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Connecting;
        }

        public override void OnDeviceDisconnected(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Disconnected;
        }

        public override void OnDeviceDisconnecting(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Disconnecting;
        }

        public override void OnDfuAborted(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Aborted;
        }

        public override void OnDfuCompleted(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Completed;
        }

        public override void OnFirmwareValidating(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Validating;
        }

        public override void OnDfuProcessStarted(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Started;
        }

        public override void OnDfuProcessStarting(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Starting;
        }

        public override void OnEnablingDfuMode(string deviceAddress)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.EnablingDfuMode;
        }

        public override void OnProgressChanged(string deviceAddress,
                                               int percent,
                                               float speed,
                                               float avgSpeed,
                                               int currentPart,
                                               int partsTotal)
        {
            if (!DfuInstallation.CheckDeviceAddress(deviceAddress)) { return; }

            DfuInstallation.State = DfuState.Uploading;

            DfuInstallation.Progress = percent / 100D;
            DfuInstallation.CurrentSpeedBytesPerSecond = speed;
            DfuInstallation.AvgSpeedBytesPerSecond = avgSpeed;
        }
    }
}