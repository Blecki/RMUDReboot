﻿using System;
using RMUD;

namespace RMUD
{
    public class Account
    {
        public String UserName;
        public String HashedPassword;
        public String Salt;
        public String AFKMessage = "AFK";

        [Newtonsoft.Json.JsonIgnore]
        public MudObject LoggedInCharacter;
    }
}
