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
	('СВ 6037 КС', 'BMW', 4, false, true),
	('ЕВ 6633 АМ', 'Bugatti', 9, true, true),
	('ОВ 5342 НК', 'Chevrolet', 3, false, false),
	('ЕН 2345 ВА', 'Lamborghini', 6, true, true),
	('СТ 5839 РА', 'BMW', 4, false, true),
	('ЕВ 9265 АР', 'Mitsubishi', 5, false, true),
	('РК 2342 МА', 'Mazda', 7, true, false),
	('В 1672 ТК', 'Porsche', 8, false, true),
	('ВР 6431 СН', 'Skoda', 5, true, true),
	('ВТ 9532 ПЕ', 'Volkswagen', 3, false, false);
	
insert into driver (id, name, address, driving_licence_valid_to, work_experience, nationality)
values
	('8803041254', 'Иван Трифонов Иванов', 'ул. Родопи 48', '2022-12-01', 5, 'Българин'),
	('9305063464', 'Есин Петрова Георгиева', 'ул. Богомил 32', '2023-04-01', 1, 'Турчин'),
	('7801024532', 'Георги Петров Владимиров', 'бул. Македония 97', '2021-12-30', 10, 'Българин'),
	('6612120534', 'Петър Петров Иванов', 'бул. Руски 54', '2020-02-01', 6, 'Българин'),
	('6804053421', 'Веселина Тодорова Трифонова', 'бул. Македония 2', '2024-01-01', 4, 'Българин'),
	('5711106412', 'Стоянка Георгиева Петрова', 'бул. 6ти септември 221', '2021-11-15', 25, 'Българин'),
	('6410108432', 'Анжелика Аманда Бенито', 'ул. Свето преображение 20', '2022-01-11', 5, 'Италианец'),
	('8603015432', 'Веселин Маринов Теодоров', 'ул. Хайделберг 1', '2021-12-25', 11, 'Българин'),
	('9511120203', 'Ивана Петрова Георгиева', 'бул. Асен Йорданов 7', '2025-01-30', 1, 'Българин'),
	('7806031236', 'Маел Матис Ромен', 'бул. Тодор Каблешков 3', '2022-05-14', 3, 'Французин');
	
insert into taxi_order (address, order_date, distance, fee, car_id, driver_id)
values
	('ул. Филип Аврамов 3', '2021-04-12 13:25:12', 10, 1.5, 2, '8803041254'),
	('бул. България 55', '2021-11-27 17:54:13', 15, 2.4, 1, '8803041254'),
	('ул. Софийски герой 4', '2021-01-01 12:15:10', 20, 5, 4, '9305063464'),
	('ул. Опълченска 35', '2021-06-22 14:11:13', 3, 4.3, 2, '7801024532'),
	('ул. Балша 14', '2021-02-02 13:44:44', 30, 10, 1, '9305063464'),
	('бул. Йерусалим 21', '2021-06-14 15:25:15', 21, 3.4, 1, '8803041254'),
	('ул. Пушкин 64', '2021-10-18 08:20:30', 12, 1.6, 2, '8803041254'),
	('ул. Монтевидео 98', '2021-09-03 10:45:15', 6, 2, 1, '9305063464'),
	('ул. Филип Аврамов 7', '2021-09-25 09:50:32', 3, 1, 3, '8803041254'),
	('бул. Чаталджа 35', '2021-10-10 18:17:10', 7, 2.6, 1, '6612120534');