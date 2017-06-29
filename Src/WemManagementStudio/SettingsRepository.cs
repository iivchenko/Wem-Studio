namespace WemManagementStudio
{
    public sealed class SettingsRepository
    {
        private bool _pathChanged;
        private bool _machinesChanged;
        private readonly Settings _settings;

        public SettingsRepository()
        {
            _settings = Serializer.Load();
        }

        public void Commit()
        {
            Serializer.Save(_settings);
        }
    }
}
