﻿namespace Users.Domain.DTOs.Responses
{
	public class TokenDTO
	{
        public string Token { get; set; }
        public TokenDTO(string token)
        {
            Token = token;
        }
	}
}
