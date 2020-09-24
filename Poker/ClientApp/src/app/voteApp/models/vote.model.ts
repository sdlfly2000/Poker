import { IClient } from './client.model';

export class Vote implements IVote {
    SessionId: string;
    Clients: any[];
    Host: any;
    IsAlive: boolean;

}

export interface IVote {
  SessionId: string;
  Clients: IClient[];
  Host: IClient;
  IsAlive: boolean;
}
