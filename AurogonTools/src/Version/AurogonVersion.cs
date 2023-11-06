using System;
using System.IO;
using System.Runtime.ConstrainedExecution;

namespace AurogonTools
{
	public class AurogonVersion
	{
		private int m_major;
		public int Major => m_major;

		private int m_minor;
		public int Minor => m_minor;

		private int m_patch;
		public int Patch => m_patch;

		private int m_build;
		public int Build => m_build;

		private static AurogonVersion m_defaultVersion = null;

		public static string VersionPath = "./version.txt";

		public static void SetVersion(int major,int minor,int patch,int build)
		{
            SetVersion(new AurogonVersion(major, minor, patch, build));
        }

		public static void SetVersion(string version)
        {
			SetVersion(new AurogonVersion(version));
        }

		private static void SetVersion(AurogonVersion ver)
        {
            if (ver > Default)
            {
                m_defaultVersion = new AurogonVersion(ver.Major, ver.Minor, ver.Patch, ver.Build);
                IOHelper.SaveFile(VersionPath, m_defaultVersion.Version);
            }
        }

        public static AurogonVersion Default
        {
            get
            {
                if (m_defaultVersion == null)
                {
					m_defaultVersion = new AurogonVersion(IOHelper.ReadFileText(VersionPath, true, "0.0.0.0"));
				}

				return m_defaultVersion;
			}
		}

		public AurogonVersion() : this(0, 0, 0, 0) { }
		
		public AurogonVersion(int major) : this(major, 0, 0, 0) { }

		public AurogonVersion(int major,int minor) : this(major, minor, 0, 0) { }

		public AurogonVersion(int major,int minor,int patch) : this(major, minor, patch, 0) { }

		public AurogonVersion(int major,int minor,int patch,int build)
		{
			m_major = major;
			m_minor = minor;
			m_patch = patch;
			m_build = build;
		}

		public AurogonVersion(string version)
		{
			if (string.IsNullOrEmpty(version))
			{
				return;
			}

			string[] versions = version.Split('.');
			if(versions == null || versions.Length == 0)
			{
				return;
			}

			if(versions.Length == 1)
			{
                int.TryParse(versions[0], out m_major);
            }
			else if(versions.Length == 2)
            {
                int.TryParse(versions[0], out m_major);
                int.TryParse(versions[1], out m_minor);
            }
            else if (versions.Length == 3)
            {
                int.TryParse(versions[0], out m_major);
                int.TryParse(versions[1], out m_minor);
                int.TryParse(versions[2], out m_patch);

            }
            else if (versions.Length >= 4)
            {
                int.TryParse(versions[0], out m_major);
                int.TryParse(versions[1], out m_minor);
                int.TryParse(versions[2], out m_patch);
                int.TryParse(versions[3], out m_build);
            }
        }

		public void AddBuild()
		{
			m_build++;
			if(m_build / 10000 > 0)
			{

			}
		}

		public void AddPatch()
		{
			m_patch++;
            if (m_patch / 1000 > 0)
            {

            }
        }

		public void AddMinor()
		{
			m_minor++;
            if (m_minor / 100 > 0)
            {


            }
        }

		public void AddMajor()
		{
			m_major++;
		}

		public string Version
		{
			get
			{
				return string.Format("{0}.{1:D2}.{2:D3}.{3:D4}",Major,Minor,Patch,Build);

            }
		}

        public override string ToString()
        {
			return $"version:{Version}";

		}

		public static bool operator <(AurogonVersion left,AurogonVersion right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            string versionLeft = left.Version;
            string versionRight = right.Version;

            int result = versionLeft.CompareTo(versionRight);

            if (result < 0)
            {
                return true;
            }

            return false;
        }

        public static bool operator >(AurogonVersion left, AurogonVersion right)
		{
			if(left == null || right == null)
			{
				return false;
			}

			string versionLeft = left.Version;
			string versionRight = right.Version;

			int result = versionLeft.CompareTo(versionRight);

            if (result > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator == (AurogonVersion left,AurogonVersion right)
		{
			if(object.Equals(left,null) || object.Equals(right,null))
			{
				return object.Equals(left, right);
			}

            return left.Major == right.Major &&
                    left.Minor == right.Minor &&
                    left.Patch == right.Patch &&
                    left.Build == right.Build;
        }

		public static bool operator != (AurogonVersion left,AurogonVersion right)
		{
			return !(left == right);
        }

        public override bool Equals(object obj)
        {
			var other = obj as AurogonVersion;
			if(other == null)
			{
				return false;
			}


            return Major == other.Major &&
                    Minor == other.Minor &&
                    Patch == other.Patch &&
                    Build == other.Build;
        }

        public override int GetHashCode()
        {
			return Version.GetHashCode();
        }
    }
}

