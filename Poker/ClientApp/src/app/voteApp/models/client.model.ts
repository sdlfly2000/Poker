export class Client implements IClient {
    ConnectionId: string;
    UserName: string;
    Vote: number;
    IsReady: boolean;
}

export interface IClient {
  ConnectionId: string;
  UserName: string;
  Vote: number;
  IsReady: boolean;
}
