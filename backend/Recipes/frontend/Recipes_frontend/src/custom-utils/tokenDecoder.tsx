import {jwtDecode} from 'jwt-decode';
import { IDecryptedToken } from '../models/types';

export const TokenDecoder = (token: string): { decoded: IDecryptedToken } => {
    const decoded : IDecryptedToken = jwtDecode<IDecryptedToken>(token);
    return {decoded};
};