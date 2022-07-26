export interface IDeploymentFrequency {
  dailyMedian: number;
  weeklyMedian: number;
  monthlyMedian: number;
  deployments: Deployment[];
}

export interface Deployment {
  date?: Date;
  count?: number;
  dayNumber?: number;
  weekNumber?: number;
  monthNumber: number;
  yearNumber: number;
  deploymentsInBucket: DeploymentsInBucket[];
}

export interface DeploymentsInBucket {
  _id: Id;
  startTime: number;
  finishTime: number;
  repository: string;
  definition: string;
  project: string;
  organization: string;
  developer: string;
  platform: string;
}

export interface Id {
  timestamp: number;
  machine: number;
  pid: number;
  increment: number;
  creationTime: Date;
}

export interface ILeadTimeChange {
  medianLeadTimeChange: number;
  weeklyLeadTimeChange: WeeklyLeadTimeChange[];
  monthlyLeadTimeChange: MonthlyLeadTimeChange[];
  changes: Change[];
}

export interface Change {
  _id: Id;
  startTime: number;
  finishTime: number;
  prSize: number;
  nrOfCommits: number;
  pullRequestId: string;
  branch: string;
  repository: string;
  project: string;
  organization: string;
  developer: string;
  platform: string;
}

export interface Id {
  timestamp: number;
  machine: number;
  pid: number;
  increment: number;
  creationTime: Date;
}

export interface MonthlyLeadTimeChange {
  key: MonthlyLeadTimeChangeKey;
  median: number;
}

export interface MonthlyLeadTimeChangeKey {
  monthNumber: number;
  yearNumber: number;
}

export interface WeeklyLeadTimeChange {
  key: WeeklyLeadTimeChangeKey;
  median: number;
}

export interface WeeklyLeadTimeChangeKey {
  weekNumber: number;
  yearNumber: number;
  monthNumber: number;
}

export interface IChangeFailureRate {
  changeFailureRate: number;
  changeFailureRateByDay: ChangeFailureRateBy[];
  changeFailureRateByWeek: ChangeFailureRateBy[];
  changeFailureRateByMonth: ChangeFailureRateByMonth[];
}

export interface ChangeFailureRateBy {
  key: ChangeFailureRateByDayKey;
  changeFailureRate: number;
}

export interface ChangeFailureRateByDayKey {
  dayNumber?: number;
  weekNumber: number;
  yearNumber: number;
  monthNumber: number;
}

export interface ChangeFailureRateByMonth {
  key: ChangeFailureRateByMonthKey;
  changeFailureRate: number;
}

export interface ChangeFailureRateByMonthKey {
  monthNumber: number;
  yearNumber: number;
}
export interface ITimeToRestoreService {
  medianRestoreServiceTime: number;
  weeklyRestoreServiceTime: WeeklyRestoreServiceTime[];
  monthlyRestoreServiceTime: MonthlyRestoreServiceTime[];
  incidents: Incident[];
}

export interface Incident {
  _id: Id;
  startTime: number;
  finishTime: number;
  jiraTicket: null | string;
  title: string;
  repository: string;
  project: string;
  organization: string;
  platform: string;
}

export interface Id {
  timestamp: number;
  machine: number;
  pid: number;
  increment: number;
  creationTime: Date;
}

export interface MonthlyRestoreServiceTime {
  key: MonthlyRestoreServiceTimeKey;
  median: number;
}

export interface MonthlyRestoreServiceTimeKey {
  monthNumber: number;
  yearNumber: number;
}

export interface WeeklyRestoreServiceTime {
  key: WeeklyRestoreServiceTimeKey;
  median: number;
}

export interface WeeklyRestoreServiceTimeKey {
  weekNumber: number;
  yearNumber: number;
  monthNumber: number;
}

export interface IInfo {
  [key: string]: IOrganizationInfo;
}
export interface IOrganizationInfo {
  [key: string]: string[];
}
