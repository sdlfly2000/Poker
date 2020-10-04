import { IClient, Client } from './client.model';

export class Vote implements IVote {
    SessionId: string;
    Clients: Client[];
    Host: Client;
    IsPublicOpen: boolean;
}

export interface IVote {
  SessionId: string;
  Clients: IClient[];
  Host: IClient;
  IsPublicOpen: boolean;
}
