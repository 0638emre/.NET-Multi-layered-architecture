using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
        //kullanıcı id şifre ile apimize verisini yolladı ve burada createtoken çalışacak ve eğer doğru ise
        //veritabanına gidecek ve bu kullanıcının list<operationclaim> e gidip claimlerini bulacak ve orada
        //Json web token üretecek, sonra onları kullanıcıya verecek.
    }
}