import { InterfaceA } from './InterfaceA';
import { InterfaceB } from './InterfaceB';

export interface ClassA extends InterfaceA, InterfaceB {
    classProp: number;
}