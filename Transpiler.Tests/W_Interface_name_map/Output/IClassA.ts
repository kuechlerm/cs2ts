import { IBase } from './IBase';
import { IIInterface } from './IIInterface';
import { IClassB } from './IClassB';

export interface IClassA extends IBase, IIInterface {
    b: IClassB;
}