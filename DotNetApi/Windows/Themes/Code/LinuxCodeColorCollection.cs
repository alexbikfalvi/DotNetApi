/* 
 * Copyright (C) 2013 Alex Bikfalvi
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Windows.Themes.Code
{
	/// <summary>
	/// A class representing the code color collection for Linux commands.
	/// </summary>
	public class LinuxCodeColorCollection : CodeColorCollection
	{
		private readonly ThemeSettings themeSettings;
		private readonly CodeColorTable colorTable;

		private static readonly string[] keywords = {
														"alias", "apropos", "apt-get", "aptitude", "aspell", "awk", "basename", "bash", "bc", "bg", "break",
														"builtin", "bzip2", "cal", "case", "cat", "cd", "cfdisk", "chgrp", "chmod", "chown", "chroot", "chkconfig",
														"cksum", "clear", "cmp", "comm", "command", "continue", "cp", "cron", "crontab", "csplit", "cut", "date",
														"dc", "dd", "ddrescue", "declare", "df", "diff", "diff3", "dig", "dir", "dircolors", "dirname", "dirs",
														"dmesg", "du", "echo", "egrep", "eject", "enable", "env", "ethtool", "eval", "exec", "exit", "expect",
														"expand", "export", "expr", "false", "fdformat", "fdisk", "fg", "fgrep", "file", "find", "fmt", "fold",
														"for", "format", "free", "fsck", "ftp", "function", "fuser", "gawk", "getopts", "grep", "groupadd", "groupdel",
														"groupmod", "groups", "gzip", "hash", "head", "help", "history", "hostname", "iconv", "id", "if", "ifconfig",
														"ifdown", "ifup", "import", "install", "jobs", "join", "kill", "killall", "less", "let", "link", "ln", "local",
														"locate", "logname", "logout", "look", "lpc", "lpr", "lprint", "lprintd", "lprintq", "lprm", "ls", "lsof",
														"make", "man", "mkdir", "mkfifo", "mkisofs", "mknod", "more", "mount", "mtools", "mtr", "mv", "mmv", "netstat",
														"nice", "nl", "nohup", "notify-send", "nslookup", "open", "op", "passwd", "paste", "pathchk", "ping", "pkill",
														"popd", "pr", "printcap", "printenv", "printf", "ps", "pushd", "pwd", "quota", "quotacheck", "quotactl", "ram",
														"rcp", "read", "readarray", "readonly", "reboot", "rename", "renice", "remsync", "return", "rev", "rm", "rmdir",
														"rsync", "screen", "scp", "sdiff", "sed", "select", "seq", "set", "sftp", "shift", "shopt", "shutdown", "sleep",
														"slocate", "sort", "source", "split", "ssh", "strace", "su", "sudo", "sum", "suspend", "sync", "tail", "tar",
														"tee", "test", "time", "timeout", "times", "touch", "top", "traceroute", "trap", "tr", "true", "tsort", "tty",
														"type", "ulimit", "umask", "umount", "unalias", "uname", "unexpand", "uniq", "units", "unset", "unshar", "until",
														"uptime", "useradd", "userdel", "usermod", "users", "uuencode", "uudecode", "v", "vdir", "vi", "vmstat", "wait",
														"watch", "wc", "whereis", "which", "while", "who", "whoami", "wget", "write", "xargs", "xdg-open", "yes"
													};

		/// <summary>
		/// Creates a new Linux code color collection instance.
		/// </summary>
		public LinuxCodeColorCollection()
		{
			// Set the theme color table.
			this.themeSettings = ToolStripManager.Renderer is ThemeRenderer ? (ToolStripManager.Renderer as ThemeRenderer).Settings : ThemeSettings.Default;
			this.colorTable = this.themeSettings.ColorTable.CodeColorTable;

			// Add the Linux tokens.

			this.Add(new Token { Regex = "#+.*$", ForegroundColor = this.colorTable.CommentForeground, BackgroundColor = this.colorTable.CommentBackground }); // Comment
			this.Add(new Token { Regex = "\"[^\"]+\"", ForegroundColor = this.colorTable.StringForeground, BackgroundColor = this.colorTable.StringBackground }); // String

			foreach (string keyword in LinuxCodeColorCollection.keywords) // Keywords
			{
				this.Add(new Token { Regex = @"\b{0}\b".FormatWith(keyword), ForegroundColor = this.colorTable.KeywordForeground, BackgroundColor = this.colorTable.KeywordBackground });
			}
		}
	}
}
