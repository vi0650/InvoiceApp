export interface DashboardState {
    label: string;
    value: number;
    type: 'currency' | 'count';
    cardClass?: string;
}