﻿
namespace QrTailor.Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public int RecoveryCode { get; set; }
        public int Status { get; set; }
        public string RefreshTokenA { get; set; }
        public DateTime RefreshTokenExpr { get; set; }
        public DateTime CreateDate { get; set; }
    }
}