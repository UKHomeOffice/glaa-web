﻿namespace GLAA.Services
{
    public interface IConstantService
    {
        int NewApplicationStatusId { get; }
        int ApplicationSubmittedOnlineStatusId { get; }
        int ApplicationSubmittedByPhoneId { get; }
    }
}
