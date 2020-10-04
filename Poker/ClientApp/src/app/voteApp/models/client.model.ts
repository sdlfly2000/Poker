export class Client implements IClient {
    ConnectionId: string;
    UserName: string;
    Vote: string;
    IsReady: boolean;
}

export interface IClient {
  ConnectionId: string;
  UserName: string;
  Vote: string;
  IsReady: boolean;
}
