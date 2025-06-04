public async UniTask<Response<$TResult>> $Name($Dto dto) =>
    await new RequestBuilder()
        .AddAuthorization()     # if $isAuth #
        .AddQuery(dto)          # if $isDto  #
        .AddBody(dto)           # if $isBody #
        .SetEndpoint($EndPoint)
        .Send<$Result>();