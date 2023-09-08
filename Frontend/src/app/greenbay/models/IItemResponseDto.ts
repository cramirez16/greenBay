import { IBidResponseDto } from './IBidResponseDto';

export interface IItemResponseDto {
  id: number;
  name: string;
  description: string;
  photoUrl: string;
  bid: number;
  price: number;
  isSellable: boolean;
  creationDate: string;
  updateDate: string;
  sellerId: number;
  sellerName: string;
  buyerId: number;
  buyerName?: string;
  bidsList: IBidResponseDto[];
}
