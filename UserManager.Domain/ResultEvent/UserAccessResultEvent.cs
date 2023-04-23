using MediatR;
using UserManager.Domain.ValueObjects;

namespace UserManager.Domain.ResultEvent;
//领域事件
public record  UserAccessResultEvent(PhoneNumber PhoneNumber,UserAccessResult Result) : INotification;