export interface IDeploymentFrequency {
  dailyMedian: number;
  weeklyMedian: number;
  monthlyMedian: number;
  weeklyDeployments: WeeklyDeployment[];
}

export interface WeeklyDeployment {
  dayNumber: number;
  weekNumber: number;
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
