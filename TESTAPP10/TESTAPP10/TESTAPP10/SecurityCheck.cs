using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace TESTAPP10
{
    public static class SecurityCheck
    {
        public static bool isTampered(string val)
        {
            if (string.IsNullOrEmpty(val))
                return false; // Don't verify blank value
            if (val =="")
                return false; // Don't verify blank value

            bool strTampered = false;

            string str = val.Replace("upper","").Replace("lower","").ToLower().Trim(); // Remove trailing spaces and convert word in lowercase. Note, its ok if the password gets converted in lowercase in this function as we are not comparing it to DB at this point. 
            if (str =="")
                return false; // Don't verify blank value

            
            if (val.Contains(" "))
                return true;


            if (val.Contains("="))
                return true;
            if (val.Contains("--"))
                return true;
            if (val.Contains("'"))
                return true;
            if (val.Contains("'--"))
                return true;
            if (val.Contains(";#"))
                return true;
            if (val.Contains("; #"))
                return true;
            if (val.Contains("/*"))
                return true;
            if (val.Contains( @"*\"))
                return true;
            if (val.Contains( @"\\"))
                return true;
            if (val.Contains("/*!"))
                return true;
            if (val.Contains("**"))
                return true;
            if (val.Contains(";"))
                return true;
            if (val.Contains("+"))
                return true;
            if (val.Contains("/0"))
                return true;
            if (val.Contains("0x"))
                return true;
            if (val.Contains("1--"))
                return true;
            if (val.Contains("1=1"))
                return true;
            if (val.Contains("1=0"))
                return true;
            if (val.Contains("1,1"))
                return true;
            if (val.Contains("' or 1=1--"))
                return true;
            if (val.Contains("' or 1=1#"))
                return true;
            if (val.Contains("' or 1=1/*"))
                return true;
            if (val.Contains("') or '1'='1--"))
                return true;
            if (val.Contains("') or ('1'='1--"))
                return true;
            if (val.Contains("';"))
                return true;
            if (val.Contains("-1"))
                return true;
            if (val.Contains("[=]"))
                return true; // ask(imp)
            if (val.Contains("<>"))
                return true; // ask(imp)
            if (val.Contains("<="))
                return true; // ask(imp)
            if (val.Contains("':'"))
                return true;
            if (val.Contains(":"))
                return true;
            if (val.Contains("(("))
                return true; // ask(imp)
            if (val.Contains("@%"))
                return true; // ask(imp)
            if (val.Contains(">>"))
                return true;

            if (val.Contains("if("))
                return true;
            if (val.Contains("true"))
                return true;
            if (val.Contains("false"))
                return true;
            if (val.Contains("(1=1)"))
                return true;
            if (val.Contains(" begin"))
                return true; // added spaces
            if (val.Contains(" end"))
                return true; // added spaces
            if (val.Contains("dbms_lock.sleep"))
                return true;

            if (val.Contains("dbo"))
                return true;
            if (val.Contains("select"))
                return true;
            if (val.Contains("drop"))
                return true;
            if (val.Contains("char("))
                return true; // ask
            if (val.Contains("||"))
                return true;
            if (val.Contains("concat"))
                return true;
            if (val.Contains("hex("))
                return true; // ask
            if (val.Contains(".ini"))
                return true;
            if (val.Contains("load_file"))
                return true;
            if (val.Contains("load"))
                return true; // ask
            if (val.Contains("file"))
                return true; // ask
            if (val.Contains("ascii"))
                return true;
            if (val.Contains("union"))
                return true;
            if (val.Contains("distinct"))
                return true;
            if (val.Contains("collate"))
                return true;
            if (val.Contains("sql_"))
                return true;
            if (val.Contains("sql"))
                return true;
            if (val.Contains(" or"))
                return true; // ask
            if (val.Contains(" and"))
                return true; // ask
            if (val.Contains("admin"))
                return true; // ask
            if (val.Contains("pass"))
                return true; // ask
            if (val.Contains("password"))
                return true; // ask
            if (val.Contains("login"))
                return true; // ask
            if (val.Contains("user"))
                return true; // ask
            if (val.Contains("md5("))
                return true; // ask
            if (val.Contains("sha1("))
                return true; // ask
            if (val.Contains("password("))
                return true; // ask
            if (val.Contains("encode("))
                return true; // ask
            if (val.Contains("compress("))
                return true; // ask
            if (val.Contains("row_count("))
                return true;
            if (val.Contains("version"))
                return true;
            if (val.Contains("having"))
                return true;
            if (val.Contains("group"))
                return true;
            if (val.Contains(" by"))
                return true; // ask
            if (val.Contains("(n)"))
                return true;
            if (val.Contains(" order"))
                return true; // ask
            if (val.Contains("sum("))
                return true; // ask
            if (val.Contains("users"))
                return true; // ask
            if (val.Contains("cast"))
                return true;
            if (val.Contains("cast("))
                return true;
            if (val.Contains("convert"))
                return true;
            if (val.Contains(" from"))
                return true;
            if (val.Contains("null"))
                return true;
            if (val.Contains(" into"))
                return true;
            if (val.Contains(" values"))
                return true;
            if (val.Contains("insert"))
                return true;
            if (val.Contains("delete"))
                return true;
            if (val.Contains("where"))
                return true;
            if (val.Contains("substring"))
                return true;
            if (val.Contains("@@"))
                return true; // ask
            if (val.Contains("systemroot"))
                return true;
            if (val.Contains("%"))
                return true; // ask
            if (val.Contains("create"))
                return true;
            if (val.Contains("bulk"))
                return true;
            if (val.Contains("queryout"))
                return true;
            if (val.Contains("bcp"))
                return true; // ask (imp)
            if (val.Contains("localhost"))
                return true;
            if (val.Contains("declare"))
                return true;
            if (val.Contains("exec"))
                return true;
            if (val.Contains(".shell"))
                return true;
            if (val.Contains("sp_oacreate"))
                return true;
            if (val.Contains("sp_oamethod"))
                return true;
            if (val.Contains("run"))
                return true; // ask
            if (val.Contains("run("))
                return true; // ask
            if (val.Contains("wscript"))
                return true;
            if (val.Contains("script"))
                return true;
            if (val.Contains("CDATA"))
                return true;
            if (val.Contains("<?xml"))
                return true;
            if (val.Contains(".exe"))
                return true;
            if (val.Contains(" dir"))
                return true; // ask(imp)
            if (val.Contains("'dir"))
                return true; // ask(imp)

            if (val.Contains("master.dbo"))
                return true;
            if (val.Contains("airtrak"))
                return true; // ask(imp)
            if (val.Contains("webairtrak"))
                return true; // ask(imp)
            if (val.Contains("cmd"))
                return true;
            if (val.Contains("ping"))
                return true;
            if (val.Contains("ping("))
                return true;
            if (val.Contains("sysmessages"))
                return true;
            if (val.Contains("sysservers"))
                return true;
            if (val.Contains("sysxlogins"))
                return true;
            if (val.Contains("sql_logins"))
                return true;
            if (val.Contains("sys."))
                return true; // ask (imp)
            if (val.Contains(".dll"))
                return true; // ask (imp)
            if (val.Contains("xp_"))
                return true; // ask(imp)
            if (val.Contains("xp_cmdshell"))
                return true;
            if (val.Contains("xp_regread"))
                return true;
            if (val.Contains("xp_regaddmultistring"))
                return true;
            if (val.Contains("xp_regdeletekey"))
                return true;
            if (val.Contains("xp_regdeletevalue"))
                return true;
            if (val.Contains("xp_regenumkeys"))
                return true;
            if (val.Contains("xp_regenumvalues"))
                return true;
            if (val.Contains("xp_regremovemultistring"))
                return true;
            if (val.Contains("xp_regwrite"))
                return true;
            if (val.Contains("hkey_local_machine"))
                return true;
            if (val.Contains("key"))
                return true; // ask        
            if (val.Contains("key-"))
                return true; // ask 
            if (val.Contains("key("))
                return true; // ask 
            if (val.Contains("system"))
                return true; // ask
            if (val.Contains("xp_servicecontrol"))
                return true;
            if (val.Contains("xp_loginconfig"))
                return true;
            if (val.Contains("xp_enumdsn"))
                return true;
            if (val.Contains("xp_availablemedia"))
                return true;
            if (val.Contains("xp_makecab"))
                return true;
            if (val.Contains("xp_ntsec_enumdomains"))
                return true;
            if (val.Contains("xp_terminate_process"))
                return true;
            if (val.Contains("xp_webserver"))
                return true;

            if (val.Contains("spid"))
                return true;
            if (val.Contains("schema"))
                return true;
            if (val.Contains("ansi"))
                return true;
            if (val.Contains("grant"))
                return true; // ask
            if (val.Contains(" default"))
                return true; // ask(imp)
            if (val.Contains(" character"))
                return true; // ask(imp)
            if (val.Contains(" set"))
                return true; // ask(imp)
            if (val.Contains("authorization"))
                return true; // ask(imp)
            if (val.Contains(" path"))
                return true; // ask(imp)
            if (val.Contains(" exist"))
                return true; // ask(imp)
            if (val.Contains("alter"))
                return true; // ask
            if (val.Contains("alter("))
                return true; // ask
            if (val.Contains("rename"))
                return true; // ask
            if (val.Contains(" else"))
                return true; // ask
            if (val.Contains(" data"))
                return true; // ask
            if (val.Contains(" owner"))
                return true; // ask
            if (val.Contains("transfer"))
                return true; // ask(imp)
            if (val.Contains("copy"))
                return true; // ask(imp)
            if (val.Contains(" join"))
                return true; // ask(imp)
            if (val.Contains(" outer"))
                return true;
            if (val.Contains(" inner"))
                return true;
            if (val.Contains("limit"))
                return true;
            if (val.Contains("';shutdown --"))
                return true;
            if (val.Contains("sp_configure"))
                return true;
            if (val.Contains("reconfigure"))
                return true;
            if (val.Contains("',1"))
                return true;
            if (val.Contains("sysobjects"))
                return true;
            if (val.Contains("object"))
                return true;
            if (val.Contains("syscolumns"))
                return true;
            if (val.Contains("not in"))
                return true;
            if (val.Contains(" top"))
                return true;
            if (val.Contains("count("))
                return true;
            if (val.Contains("tmp_sys_tmp"))
                return true;

            if (val.Contains("information_schema"))
                return true;
            if (val.Contains("table"))
                return true;
            if (val.Contains("delay"))
                return true;
            if (val.Contains("delay("))
                return true;
            if (val.Contains("time"))
                return true;
            if (val.Contains("time("))
                return true;
            if (val.Contains("sleep"))
                return true;
            if (val.Contains("sleep("))
                return true;
            if (val.Contains("wait"))
                return true;
            if (val.Contains("wait("))
                return true;
            if (val.Contains("isnull("))
                return true;

            if (val.Contains("top 0"))
                return true;
            if (val.Contains("waitfor delay"))
                return true;
            if (val.Contains("benchmark"))
                return true;
            if (val.Contains("root"))
                return true;
            if (val.Contains("dbms_pipe"))
                return true;
            if (val.Contains("nvl("))
                return true;

            if (val.Contains("substr("))
                return true;
            if (val.Contains("left("))
                return true;
            if (val.Contains("right("))
                return true;

            if (val.Contains("function"))
                return true;
            if (val.Contains("kernel32"))
                return true;
            if (val.Contains("view"))
                return true;
            if (val.Contains("procedure"))
                return true;
            if (val.Contains("index"))
                return true;
            if (val.Contains("update"))
                return true;
            if (val.Contains("sys.dbms_ldap.init"))
                return true;
            if (val.Contains("varchar"))
                return true;
            if (val.Contains("dirtree"))
                return true;
            if (val.Contains(".txt"))
                return true;
            if (val.Contains("dual"))
                return true;
            if (val.Contains("dual("))
                return true;

            if (val.Contains("consol"))
                return true;
            if (val.Contains("write"))
                return true;
            if (val.Contains("query"))
                return true;
            if (val.Contains("response"))
                return true;
            if (val.Contains("request"))
                return true;

            return false;
        }
    }
}
