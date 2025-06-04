public async UniTask<Response<$TResult>> $Name($Dto dto) =>
    await new RequestBuilder()
        .AddAuthorization()     # has-envar $isAuth #
        .AddQuery(dto)          # has-envar $isDto  #
        .AddBody(dto)           # has-envar $isBody #
        .SetEndpoint($EndPoint)
        .Send<$Result>();