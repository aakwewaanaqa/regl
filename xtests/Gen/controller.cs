//#!
//#!add-evcm Task<Response<([a-zA-Z]+?)>> $1 TResult
    /// <summary>
    ///     讀取在 firestore 中 users 路徑以下的資料
    /// </summary>
    [Route("getUsers/doc")]
    [ReturnType(typeof(object))]
#if DEV
    [GenReturn(typeof(Response<>))]
    [GenGeneric("T")]
#endif
//#!tpl 1 controller-tpl.sh
    public async Task<Response<object>> GetUsersDoc([FromBody] FirestoreDocDto dto) {
        try {
            Response<UserInfo> userRsp;
            Response<PathLike> pathRsp;
            Response<object>   docRsp;

            userRsp = HttpContext.GetItem<Response<UserInfo>>();
            userRsp = await auth.CheckUserAlive(userRsp);
            pathRsp = await aimyFirestore.CombinePathFromUserInfo((userRsp, dto.path));
            pathRsp = await aimyFirestore.CheckPathIsDocument(pathRsp);
            docRsp  = await aimyFirestore.GetDocument(pathRsp);
            return docRsp;
        }
        catch (Exception e) {
            return Response<object>.Exception(e);
        }
    }