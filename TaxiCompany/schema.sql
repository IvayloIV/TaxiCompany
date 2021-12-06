create table car (
	id bigserial primary key,
	registration_plate varchar(31) not null unique check(length(registration_plate) > 3),
	brand varchar(63) not null check(length(brand) > 1),
	passenger_seats smallint not null check(passenger_seats > 2 and passenger_seats < 11),
	big_boot_space boolean not null default false,
	technical_review boolean not null default false
);

create table driver (
	id char(10) check(length(id) = 10) primary key,
	name varchar(255) not null check(length(name) > 5),
	address varchar(511),
	driving_licence_valid_to date not null,
	work_experience smallint not null default 0 check(work_experience >= 0),
	nationality varchar(255) not null check(length(nationality) > 2)
);

create table taxi_order (
	id bigserial primary key,
	address varchar(511) not null check(length(address) > 4),
	order_date timestamp not null default now(),
	distance real not null check (distance > 0),
	fee decimal not null check (fee > 0),
	car_id bigint not null references car(id),
	driver_id char(10) not null references driver(id)
);

insert into car (registration_plate, brand, passenger_seats, big_boot_space, technical_review)
values
	('�� 6037 ��', 'BMW', 4, false, true),
	('�� 6633 ��', 'Bugatti', 9, true, true),
	('�� 5342 ��', 'Chevrolet', 3, false, false),
	('�� 2345 ��', 'Lamborghini', 6, true, true),
	('�� 5839 ��', 'BMW', 4, false, true),
	('�� 9265 ��', 'Mitsubishi', 5, false, true),
	('�� 2342 ��', 'Mazda', 7, true, false),
	('� 1672 ��', 'Porsche', 8, false, true),
	('�� 6431 ��', 'Skoda', 5, true, true),
	('�� 9532 ��', 'Volkswagen', 3, false, false);
	
insert into driver (id, name, address, driving_licence_valid_to, work_experience, nationality)
values
	('8803041254', '���� �������� ������', '��. ������ 48', '2022-12-01', 5, '��������'),
	('9305063464', '���� ������� ���������', '��. ������� 32', '2023-04-01', 1, '������'),
	('7801024532', '������ ������ ����������', '���. ��������� 97', '2021-12-30', 10, '��������'),
	('6612120534', '����� ������ ������', '���. ����� 54', '2020-02-01', 6, '��������'),
	('6804053421', '�������� �������� ���������', '���. ��������� 2', '2024-01-01', 4, '��������'),
	('5711106412', '������� ��������� �������', '���. 6�� ��������� 221', '2021-11-15', 25, '��������'),
	('6410108432', '�������� ������ ������', '��. ����� ������������ 20', '2022-01-11', 5, '���������'),
	('8603015432', '������� ������� ��������', '��. ���������� 1', '2021-12-25', 11, '��������'),
	('9511120203', '����� ������� ���������', '���. ���� �������� 7', '2025-01-30', 1, '��������'),
	('7806031236', '���� ����� �����', '���. ����� ��������� 3', '2022-05-14', 3, '���������');
	
insert into taxi_order (address, order_date, distance, fee, car_id, driver_id)
values
	('��. ����� ������� 3', '2021-04-12 13:25:12', 10, 1.5, 2, '8803041254'),
	('���. �������� 55', '2021-11-27 17:54:13', 15, 2.4, 1, '8803041254'),
	('��. �������� ����� 4', '2021-01-01 12:15:10', 20, 5, 4, '9305063464'),
	('��. ���������� 35', '2021-06-22 14:11:13', 3, 4.3, 2, '7801024532'),
	('��. ����� 14', '2021-02-02 13:44:44', 30, 10, 1, '9305063464'),
	('���. ��������� 21', '2021-06-14 15:25:15', 21, 3.4, 1, '8803041254'),
	('��. ������ 64', '2021-10-18 08:20:30', 12, 1.6, 2, '8803041254'),
	('��. ���������� 98', '2021-09-03 10:45:15', 6, 2, 1, '9305063464'),
	('��. ����� ������� 7', '2021-09-25 09:50:32', 3, 1, 3, '8803041254'),
	('���. �������� 35', '2021-10-10 18:17:10', 7, 2.6, 1, '6612120534');