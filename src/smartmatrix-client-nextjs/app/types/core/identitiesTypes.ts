export interface SysUser_OutputPayload {
    id: number;
    guid: string | null;
    isDeleted: boolean;
    status: string | null;
    createdAt: string | null;
    createdBy: string | null;
    modifiedAt: string | null;
    modifiedBy: string | null;
    deletedAt: string | null;
    deletedBy: string | null;
    partitionKey: string | null;
    type: string | null;
    userName: string | null;
    displayName: string | null;
    givenName: string | null;
    surname: string | null;
    email: string | null;
    logins: SysLogin_OutputPayload[];
    roles: SysRole_OutputPayload[];
}

export interface SysLogin_OutputPayload {
    id: number;
    guid: string | null;
    isDeleted: boolean;
    status: string | null;
    createdAt: string | null;
    createdBy: string | null;
    modifiedAt: string | null;
    modifiedBy: string | null;
    deletedAt: string | null;
    deletedBy: string | null;
    partitionKey: string | null;
    sysUserId: number;
    loginProvider: string | null;
    loginType: string | null;
    loginName: string | null;
    description: string | null;
    pictureUrl: string | null;
}

export interface SysRole_OutputPayload {
    id: number;
    guid: string | null;
    isDeleted: boolean;
    status: string | null;
    createdAt: string | null;
    createdBy: string | null;
    modifiedAt: string | null;
    modifiedBy: string | null;
    deletedAt: string | null;
    deletedBy: string | null;
    partitionKey: string | null;
    type: string | null;
    category: string | null;
    roleCode: string | null;
    roleName: string | null;
    description: string | null;
}