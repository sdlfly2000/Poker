import { Client } from './client.model';

export interface Vote {
  SessionId: string;
  Clients: Client[];
  Host: Client;
  IsAlive: boolean;
}
