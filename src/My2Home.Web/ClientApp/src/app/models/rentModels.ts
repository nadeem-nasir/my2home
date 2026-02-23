import { ITenantModels} from './tenantModels'
export interface IRentModels {

  rentId: number;
  rentMonth: string;
  rentYear: number;
  rentStatus: string;
  rentAmount: number;
  rentBedNumber: number;
  rentCreationDate:string;
  rentDueDateTime: string;
  rentDateTime: string;
  rentHostelId: number;
  rentTenantId: number;
  rentTenant: ITenantModels;
}
